using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 10f;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Siempre delante de la c�mara
        transform.position = cameraTransform.position + cameraTransform.forward * distance;

        // Hacer que el quad mire hacia la c�mara
        transform.rotation = cameraTransform.rotation;
    }
}