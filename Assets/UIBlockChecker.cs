using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlockChecker : MonoBehaviour
{
    public void CheckUIBlocking()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            Debug.Log("Elementos bloqueando el botón:");
            foreach (var result in results)
            {
                Debug.Log(result.gameObject.name);
            }
        }
        else
        {
            Debug.Log("No hay elementos bloqueando.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckUIBlocking();
        }
    }
}
