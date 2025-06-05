Shader "Custom/SpaceWithStarsTransparent"
{
    Properties
    {
        _MainTex ("Gradient Texture", 2D) = "white" {}
        _Speed ("Scroll Speed", Float) = 0.1
        _Scale ("Noise Scale", Float) = 5.0
        _StarDensity ("Star Density", Float) = 0.98
        _StarBrightness ("Star Brightness", Float) = 1.5
        _AlphaThreshold ("Transparency Threshold", Range(0,1)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Speed;
            float _Scale;
            float _StarDensity;
            float _StarBrightness;
            float _AlphaThreshold;
            float4 _MainTex_ST;
            float2 _CameraOffset;

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
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _Speed;
                float2 uv = i.uv + _CameraOffset * 0.04;

                // Nebula noise
                float2 nebulaUV = uv * _Scale + float2(t * 0.1, t * 0.05);
                float nebulaNoise = noise(nebulaUV);

                float2 gradUV = float2(nebulaNoise, 0.5);
                fixed4 nebulaColor = tex2D(_MainTex, gradUV);

                // Stars
                float2 starUV = uv * 100.0;
                float starNoise = hash(floor(starUV));
                float star = step(_StarDensity, starNoise);
                star *= _StarBrightness;
                fixed4 starColor = fixed4(star, star, star, star);

                // Combine
                fixed4 finalColor = nebulaColor + starColor;

                // Compute brightness to drive alpha
                float brightness = dot(finalColor.rgb, float3(0.299, 0.587, 0.114)); // luminancia perceptual
                finalColor.a = saturate((brightness - _AlphaThreshold) / (1.0 - _AlphaThreshold));

                return finalColor;
            }
            ENDCG
        }
    }
}
