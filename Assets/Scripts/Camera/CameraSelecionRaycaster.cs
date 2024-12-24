using UnityEngine;
using UnityEngine.EventSystems;

public class CameraSelectionRaycaster : MonoBehaviour
{
    private Camera _camera;
    private SelectorSize _selectorSize;
    private ITargetSelectable _currentTarget;
    public bool selectionActivated = true;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _selectorSize = GetComponent<SelectorSize>();
    }

    private void Update()
    {
        if (!selectionActivated) return;

        if (IsPointerOverUI()) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            HandleRaycastHit(hit);
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        Transform hitTransform = hit.transform;

        if (hitTransform.TryGetComponent<ITargetSelectable>(out _currentTarget) && Input.GetMouseButtonDown(0))
        {
            _currentTarget.OnSelectTarget();

            if (hitTransform.CompareTag("Ship"))
            {
                HandleShipSelection(hitTransform);
            }
        }
    }

    private void HandleShipSelection(Transform shipTransform)
    {
        var shipBehaviour = shipTransform.GetComponent<ShipBehaiviour>();

        if (shipBehaviour != null)
        {
            _selectorSize.Initialize(shipTransform.gameObject, shipBehaviour.ShipSize);
            TurnManager.Instance.SetTarget(shipBehaviour);
        }
    }
}
