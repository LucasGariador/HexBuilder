using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager instance;
    public static TurnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TurnManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("TurnManager");
                    instance = go.AddComponent<TurnManager>();
                }
            }
            return instance;
        }
    }

    public List<ShipBehaiviour> turnOrder;

    public TurnManager()
    {
        turnOrder = new List<ShipBehaiviour>();
    }

    public void InitializeTurnOrder(List<ShipBehaiviour> entities)
    {
        turnOrder = entities.OrderByDescending(e => e.Speed).ToList();

        Debug.Log("Initial Turn Order:");
        foreach (var entity in turnOrder)
        {
            Debug.Log($"{entity.ShipName} with Speed: {entity.Speed}");
        }
    }

    public void UpdateTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(e => e.Speed).ToList();
    }

    public void NextTurn()
    {
        if (turnOrder.Count == 0) return;

        ShipBehaiviour currentEntity = turnOrder[0];
        currentEntity.TakeTurn();

        // Remove the current entity and reinsert it at the end without reordering
        turnOrder.RemoveAt(0);
        turnOrder.Add(currentEntity);
    }

    public void RemoveEntity(ShipBehaiviour shipBehaiviour)
    {
        turnOrder.Remove(shipBehaiviour);
        if (turnOrder.Count == 1)
        {
            Debug.Log("Combat has ended!");
        }
    }
}


