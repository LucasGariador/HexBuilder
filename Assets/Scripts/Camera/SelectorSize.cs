using UnityEngine;

public enum ShipSize { Small, Medium, Large }
public class SelectorSize : MonoBehaviour
{
    public ShipSize shipSize;
    [SerializeField]
    private GameObject selectorObject;

    [Tooltip("ShipsSize")]
    [SerializeField]
    private Vector3 small;
    [SerializeField]
    private Vector3 medium;
    [SerializeField]
    private Vector3 large;

    [Tooltip("Materials")]
    [SerializeField]
    private Material enemyMat;
    [SerializeField]
    private Material playerMat;

    public void Initialize(GameObject target, ShipSize size, bool isplayer)
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
        Material mat = isplayer ? playerMat : enemyMat;
        foreach(Renderer r in selectorObject.GetComponentsInChildren<Renderer>())
        {
            r.material = mat;
        }


        selectorObject.transform.position = target.transform.position;
    }
}
