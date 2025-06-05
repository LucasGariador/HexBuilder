using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class DiceRoller : MonoBehaviour
{
    public float spinDuration = 2f;
    public float transitionDuration = 0.5f;
    public float spinSpeed = 360f; // más controlado
    public float rollSpeed = 1f; // velocidad de rodadura
    Vector2[] diceDirections = {
        new Vector2(-1, 1), // posición 1
        new Vector2(1, 1), // posición 2
        new Vector2(-1, -1), // posición 3
        new Vector2(1, -1), // posición 4
    };

    public DiceFaceRotations rotations;

    private Coroutine rollCoroutine;

    public void RollTo(int faceIndex)
    {
        if (rollCoroutine != null)
            StopCoroutine(rollCoroutine);

        rollCoroutine = StartCoroutine(RollCoroutine(faceIndex));
    }

    private IEnumerator RollCoroutine(int targetFace)
    {
        // 1. Elegir un eje de rotación coherente
        Vector3 spinAxis = Random.onUnitSphere.normalized;
        int direction = Random.Range(0, 4);

        float timer = 0f;
        AudioManager.Instance.PlaySFX("RollingMetal"); // Reproducir el sonido de lanzamiento
        while (timer < spinDuration/3)
        {
            transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);
            transform.Translate(diceDirections[direction] * rollSpeed * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f; // Reiniciar el temporizador

        AudioManager.Instance.StopSFX(); // Detener el sonido de lanzamiento
        AudioManager.Instance.PlaySFX("RollingMetal"); // Reproducir el sonido de lanzamiento
        spinAxis = Random.onUnitSphere.normalized;
        direction = Random.Range(0, 4);

        while (timer < spinDuration/3)
        {
            transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);
            Vector3 target = new Vector3(0, 0, transform.position.z); // mantenemos la Z actual
            Vector3 directionCenter = (target - transform.position).normalized;
            transform.position += directionCenter * rollSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f; // Reiniciar el temporizador 


        AudioManager.Instance.StopSFX(); // Detener el sonido de lanzamiento
        AudioManager.Instance.PlaySFX("RollingMetal"); // Reproducir el sonido de lanzamiento
        spinAxis = Random.onUnitSphere.normalized;

        while (timer < spinDuration/3)
        {
            transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);
            transform.Translate(diceDirections[direction] * rollSpeed * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }

        // Fase 2: Suavizado hacia la rotación final
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = rotations.GetRotationForValue(targetFace);
        timer = 0f;

        while (timer < transitionDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timer / transitionDuration);
            Vector3 target = new Vector3(0, 0, transform.position.z); // mantenemos la Z actual
            Vector3 directionCenter = (target - transform.position).normalized;
            transform.position += directionCenter * rollSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.StopSFX(); // Detener el sonido de lanzamiento
        //transform.position = Vector3.zero; // Asegura que el dado esté en el centro
        transform.rotation = endRotation; // asegura precisión final

        //Efecto de particulas!
        yield return new WaitForSeconds(1f); // Espera un momento para mostrar el resultado

        //Aqui quiero agregar un tiempo de espera antes de que el dado se dirija al centro
        while (Vector3.Distance(transform.position, Vector3.zero) >= 0.1)
        {
            Vector3 target = new Vector3(0, 0, transform.position.z); // mantenemos la Z actual
            Vector3 directionCenter = (target - transform.position).normalized;
            transform.position += directionCenter * 3 * Time.deltaTime;
            yield return null;
        }

    }
}

