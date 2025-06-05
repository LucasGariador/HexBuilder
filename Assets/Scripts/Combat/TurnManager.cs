using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager instance;

    private ShipBehaiviour SelectedTarget;
    public ShipBehaiviour CurrentShip { get; private set; }
    public static TurnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<TurnManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("TurnManager");
                    instance = go.AddComponent<TurnManager>();
                }
            }
            return instance;
        }
    }

    bool gameisOver= false;

    public List<ShipBehaiviour> turnOrder;

    public TurnManager()
    {
        turnOrder = new List<ShipBehaiviour>();
        Debug.Log("");
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
        if (turnOrder.Count == 0 || gameisOver) return;

        CurrentShip = turnOrder[0];
        CurrentShip.TakeTurn();

        // Remove the current entity and reinsert it at the end without reordering
        turnOrder.RemoveAt(0);
        turnOrder.Add(CurrentShip);


        // Verifica el estado del combate
        CheckCombatEnd();
    }

    public void RemoveEntity(ShipBehaiviour shipBehaiviour)
    {
        if (turnOrder.Contains(shipBehaiviour))
        {
            turnOrder.Remove(shipBehaiviour);
        }
    }


    // Devuelve true si todos los enemigos han sido eliminados
    public bool AreAllEnemiesDefeated()
    {
        return !turnOrder.Any(ship => ship is Enemy && ship.Health > 0);
    }

    // Devuelve true si todos los jugadores han sido eliminados
    public bool AreAllPlayersDefeated()
    {
        return !turnOrder.Any(ship => ship is Player && ship.Health > 0);
    }


    public void CheckCombatEnd()
    {
        if (AreAllEnemiesDefeated())
        {
            Debug.Log("Victory! All enemies defeated.");
            EndCombat(true); // true para indicar victoria del jugador
        }
        else if (AreAllPlayersDefeated())
        {
            Debug.Log("Defeat! All players defeated.");
            EndCombat(false); // false para indicar derrota del jugador
        }
    }

    // Método para finalizar el combate
    private void EndCombat(bool playerWon)
    {
        gameisOver = true;
        if (playerWon)
        {
            // Lógica para la victoria del jugador
            Debug.Log("You win!");
        }
        else
        {
            // Lógica para la derrota del jugador
            Debug.Log("Game over.");
        }

        turnOrder.Clear();
    }

    public void SetTarget(ShipBehaiviour ship)
    {
        SelectedTarget = ship;
    }

    public ShipBehaiviour GetTarget()
    {
        return SelectedTarget;
    }

}


