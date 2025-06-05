using System.Collections;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeDuration = 1f; // Duración del temblor
    public float magnitude = 0.1f;   // Intensidad del temblor
    public float speed = 10f;        // Velocidad del temblor

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        Shake();
    }

    public void Shake()
    {
        if (!isShaking)
        {
            originalPosition = transform.localPosition;
            StartCoroutine(DoShake());
        }
    }

    private IEnumerator DoShake()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float x = Mathf.Sin(Time.time * speed) * magnitude;
            float y = Mathf.Cos(Time.time * speed) * magnitude;
            float z = Mathf.Sin(Time.time * speed * 0.5f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
