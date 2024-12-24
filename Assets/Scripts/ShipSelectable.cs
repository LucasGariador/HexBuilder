using UnityEngine;

public class ShipSelectable : MonoBehaviour, ITargetSelectable
{
    public void OnHighlightTarget()
    {
        Debug.Log(this.gameObject.name + " is highlighted!");
    }

    public void OnSelectTarget()
    {
        Debug.Log(this.gameObject.name + " is selected!");
    }
}
