using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public HexGrid hexGrid;
    [SerializeField]
    float cameraSpeed = 10f;
    [SerializeField]
    float zoomSpeed = 100f;
    [SerializeField]
    float minCamHeight = 5f;
    [SerializeField]
    float maxCamHeight = 50f;
    [SerializeField]
    float smoothTime = 0.1f;

    private Vector3 minCameraPosition;
    private Vector3 maxCameraPosition;

    private Vector3 velocity = Vector3.zero;

    private Vector3 targetPosition;
    [HideInInspector]
    public bool isReady = false;

    void Start()
    {
        Invoke(nameof(LateStart), 1f);
    }

    void LateStart()
    {
        CalculateCameraLimits();
        CenterCameraOnGrid(TileManager.instance.currentTile);
        isReady = true;
    }


    void Update()
    {
        if (!isReady)
        {
            return;
        }

        // Input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Movimiento objetivo
        targetPosition += inputDirection * cameraSpeed * Time.deltaTime;

        // Zoom objetivo
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float targetY = Mathf.Clamp(targetPosition.y - zoomInput * zoomSpeed, minCamHeight, maxCamHeight);
        targetPosition.y = Mathf.Lerp(targetPosition.y, targetY, Time.deltaTime * 10f); // 10f puede ajustarse

        // Clampeo de límites
        targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraPosition.x, maxCameraPosition.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minCameraPosition.z, maxCameraPosition.z);

        // Movimiento suave hacia la posición objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    }


    void CenterCameraOnGrid(HexTile tile)
    {
        if (tile == null)
        {
            Debug.LogError("Tile reference not set in CameraController!");
            return;
        }
        float height = transform.position.y;

        targetPosition = tile.transform.position + new Vector3(0, height, -5f);
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
        maxCamHeight = (hexGrid.gridSize.x + hexGrid.gridSize.y) / 2;
    }

    public void StopCameraController()
    {
        isReady = false;
        GetComponent<CameraSelectionRaycaster>().selectionActivated = false;
    }

    public void StartCameraController()
    {
        isReady = true;
        GetComponent<CameraSelectionRaycaster>().selectionActivated = true;
        CalculateCameraLimits();
        CenterCameraOnGrid(TileManager.instance.currentTile);
    }
}

