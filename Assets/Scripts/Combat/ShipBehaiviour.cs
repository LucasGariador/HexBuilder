using System.Collections.Generic;
using UnityEngine;

public abstract class ShipBehaiviour : MonoBehaviour
{
    public ShipData shipData;
    public List<Action> actions;

    public int Health { get; private set; }
    public int Speed { get; private set; }

    public string ShipName { get; private set; }

    public ShipSize ShipSize { get; private set; }
    protected void Start()
    {
        if (shipData != null)
        {
            Health = shipData.health;
            Speed = shipData.speed;
            ShipName = shipData.shipName;
            ShipSize = shipData.shipSize;
        }
        else
        {
            Debug.LogError("ShipData is not assigned to " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log($"{ShipName} has died.");
            TurnManager.Instance.RemoveEntity(this);
            this.GetDestroyed();
        }
    }
   
    public void GetDestroyed()
    {
        //Animaciones de destruccion y sonido
    }

    public void ModifySpeed(int amount)
    {
        Speed += amount;
        // Notify TurnManager to update the turn order
        TurnManager.Instance.UpdateTurnOrder();
    }

    public abstract void TakeTurn();
}
