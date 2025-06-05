using System;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string actionName;
    public int actionCost;

    public abstract void Execute(ShipBehaiviour user, ShipBehaiviour target);

    internal void Invoke()
    {
        throw new NotImplementedException();
    }
}
