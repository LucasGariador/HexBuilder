using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ship Data", order = 51)]
public class ShipData : ScriptableObject
{
    public int health;
    public int speed;
    public string shipName;
}

