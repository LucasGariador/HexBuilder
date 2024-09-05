using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class CombatUI : MonoBehaviour
{
    public GameObject targetPanel;  // Panel que contiene los botones de selección de objetivos
    public Button attackButton;
    private ShipBehaiviour selectedEntity;
    private ShipBehaiviour currentPlayer;

    void Start()
    {
        attackButton.onClick.AddListener(OnAttackButtonPressed);
    }

    public void SetCurrentPlayer(ShipBehaiviour player)
    {
        currentPlayer = player;
        UpdateTargetPanel();
    }

    void UpdateTargetPanel()
    {
        // Limpiar los botones anteriores
        foreach (Transform child in targetPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Crear un botón para cada enemigo
        foreach (var entity in FindObjectsOfType<Enemy>())
        {
            GameObject buttonGO = new GameObject("TargetButton");
            Button button = buttonGO.AddComponent<Button>();
            Text buttonText = buttonGO.AddComponent<Text>();
            buttonText.text = entity.ShipName;
            button.onClick.AddListener(() => SelectTarget(entity));
            buttonGO.transform.SetParent(targetPanel.transform);
        }
    }

    void SelectTarget(ShipBehaiviour entity)
    {
        selectedEntity = entity;
        Debug.Log("Selected target: " + entity.ShipName);
    }

    void OnAttackButtonPressed()
    {
        if (currentPlayer != null && selectedEntity != null)
        {
            if (currentPlayer.actions.Count > 0)
            {
                currentPlayer.actions[0].Execute(currentPlayer, selectedEntity);
                TurnManager.Instance.NextTurn();
            }
        }
        else
        {
            Debug.LogError("Current player or selected entity is null");
        }
    }
}
