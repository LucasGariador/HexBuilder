using UnityEngine;

public class CameraSelecionRaycaster : MonoBehaviour
{
    private Camera _camera;
    private ITargetSelectable target;
    public bool selectionActivated = true;
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (TileManager.instance.settlementIsUp)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (Physics.Raycast(ray, out RaycastHit hit) && !isOverUI)
        {
            Transform objectHit = hit.transform;

            if (objectHit.TryGetComponent<ITargetSelectable>(out target))
            {
                if (selectionActivated)
                {
                    target.OnHighlightTarget();
                }
                if(Input.GetMouseButtonDown(0))
                {
                    target.OnSelectTarget();
                }
            }
        }
    }
}
