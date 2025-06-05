using UnityEngine;
[ExecuteAlways]
public class DiceRotationCapture : MonoBehaviour
{
    public DiceFaceRotations data;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            data.faceRotations.Add(transform.rotation);
            Debug.Log("Guardada rotación: " + transform.rotation);
        }
    }
}
