using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string actionName;
    public int actionCost;

    public abstract void Execute(ShipBehaiviour user, ShipBehaiviour target);
}
