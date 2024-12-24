using UnityEngine;

public enum ShipSize { Small, Medium, Large }
public class SelectorSize : MonoBehaviour
{
    public ShipSize shipSize;
    [SerializeField]
    private GameObject selectorObject;

    [Tooltip("ShipsSize")]
    public Vector3 small;
    public Vector3 medium;
    public Vector3 large;

    public void Initialize(GameObject target, ShipSize size)
    {
        shipSize = size;
        if (target != null)
        {
            switch (shipSize)
            {
                case ShipSize.Small:
                    selectorObject.transform.localScale = small;
                    break;
                case ShipSize.Medium:
                    selectorObject.transform.localScale = medium;
                    break;
                case ShipSize.Large:
                    selectorObject.transform.localScale = large;
                    break;
                default:
                    break;
            }
        }

        //Moverlo a la posicion del objetivo
        selectorObject.transform.position = target.transform.position;
    }
}
