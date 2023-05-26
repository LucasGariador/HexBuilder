using UnityEngine;

public class CameraSelecionRaycaster : MonoBehaviour
{
    private Ray ray;
    private Camera _camera;
    private ITargetSelectable target;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.TryGetComponent<ITargetSelectable>(out target))
            {
                target.OnHighlightTarget();
                if(Input.GetMouseButtonDown(0))
                {
                    target.OnSelectTarget();
                }
            }
        }
    }
}
