using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 10f;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Siempre delante de la cámara
        transform.position = cameraTransform.position + cameraTransform.forward * distance;

        // Hacer que el quad mire hacia la cámara
        transform.rotation = cameraTransform.rotation;
    }
}