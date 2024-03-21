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
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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
