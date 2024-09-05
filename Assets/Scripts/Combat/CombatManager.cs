using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public List<ShipBehaiviour> entities;

    void Start()
    {
        // Inicializar entidades
        entities = new List<ShipBehaiviour>();

        // Ejemplo de cómo podrías inicializar tus entidades en el editor
        foreach (var entityGO in GameObject.FindGameObjectsWithTag("Ship"))
        {
            ShipBehaiviour entity = entityGO.GetComponent<ShipBehaiviour>();
            if (entity != null)
            {
                entities.Add(entity);
            }
        }

        // Configurar el TurnManager
        TurnManager.Instance.InitializeTurnOrder(entities);
    }

    public void NextTurn()
    {
        TurnManager.Instance.NextTurn();
    }
}
