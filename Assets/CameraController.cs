using UnityEngine;

public class CameraController : MonoBehaviour
{
    public HexGrid hexGrid;
    public float cameraSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minCamHeight = 5f;
    public float maxCamHeight = 50f;

    private Vector3 minCameraPosition;
    private Vector3 maxCameraPosition;
    
    void Start()
    {
        CalculateCameraLimits();
    }

    void Update()
    {
        // Cam Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.position += cameraSpeed * Time.deltaTime * moveDirection;
        // Cam Zoom
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = zoomInput * zoomSpeed * Time.deltaTime;
        float newZoom = Mathf.Clamp(transform.position.y - zoomAmount, minCamHeight, maxCamHeight);
        transform.position = new Vector3(transform.position.x, newZoom, transform.position.z);

        // clamp Limits
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minCameraPosition.x, maxCameraPosition.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minCameraPosition.z, maxCameraPosition.z);
        transform.position = clampedPosition;

    }

    void CalculateCameraLimits()
    {
        if (hexGrid == null)
        {
            Debug.LogError("HexGrid reference not set in CameraController!");
            return;
        }

        // Obtener el tamaño del grid
        Vector2Int gridSize = hexGrid.gridSize;

        // Calcular las posiciones mínima y máxima de la cámara
        float halfWidth = gridSize.x * hexGrid.radius * Mathf.Sqrt(3f);
        float halfHeight = gridSize.y * hexGrid.radius * 1.5f;
        minCameraPosition = new Vector3(0f, 0f, -halfHeight);
        maxCameraPosition = new Vector3(halfWidth, 0f, 0f);
    }
}

