Shader "Custom/GalaxySDF"
{
    Properties
    {
        _MainTex ("Color Gradient", 2D) = "white" {}
        _Arms ("Arm Count", Float) = 5.0
        _Twist ("Twist Amount", Float) = 2.0
        _Density ("Star Density", Float) = 40.0
        _TimeSpeed ("Rotation Speed", Float) = 0.2
        _CoreIntensity ("Core Brightness", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Arms;
            float _Twist;
            float _Density;
            float _TimeSpeed;
            float _CoreIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0; // Centro en (0,0)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float t = _Time.y * _TimeSpeed;

                float r = length(uv);
                float angle = atan2(uv.y, uv.x);

                // Agregamos rotación tipo espiral
                float spiral = sin(angle * _Arms + r * _Twist - t);

                // Agregamos ruido para irregularidad
                float2 npos = uv * _Density;
                float n = noise(npos);

                float brightness = spiral * 0.5 + 0.5;
                brightness *= n;

                // Núcleo brillante al centro
                float core = exp(-r * 10.0) * _CoreIntensity;
                brightness += core;

                // Color desde gradiente según brillo
                float2 gradUV = float2(saturate(brightness), 0.5);
                fixed4 col = tex2D(_MainTex, gradUV);
                col.a = brightness;

                return col;
            }
            ENDCG
        }
    }
}
