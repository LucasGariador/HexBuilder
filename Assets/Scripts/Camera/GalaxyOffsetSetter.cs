using UnityEngine;

[ExecuteAlways]
public class GalaxyOffsetSetter : MonoBehaviour
{
    public Material galaxyMaterial;
    public Transform cameraTransform;

    void Update()
    {
        if (galaxyMaterial != null && cameraTransform != null)
        {
            Vector2 camXZ = new Vector2(cameraTransform.position.x, cameraTransform.position.z);
            galaxyMaterial.SetVector("_CameraOffset", camXZ);
        }
    }
}
