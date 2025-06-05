using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public static EventUIManager Instance;

    [Header("UI References")]
    public GameObject eventPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI diceResult;
    [SerializeField] private Image eventImage;
    [SerializeField] private Button resolveEvent;
    [SerializeField] private Button rollButton;
    [SerializeField] private DiceRoller diceRoller;
    [SerializeField] private GameObject rollcamera;

    private System.Action onResolvedCallback;
    public EventSO currentEvent;
    private bool eventFinished;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        eventPanel.SetActive(false);
    }

    public void ShowEvent(EventSO eventSO, System.Action onResolved, PlayerOnWorldMap playerOnWorldMap)
    {
        onResolvedCallback = onResolved;

        currentEvent = eventSO;
        titleText.text = eventSO.eventName;
        descriptionText.text = eventSO.eventDescription;
        eventImage.sprite = eventSO.eventIcon;

        statsText.text =
    $"<b>Player Stats</b>\n" +
    $"Constitution: {playerOnWorldMap.stats.constitution}\n" +
    $"Dexterity:    {playerOnWorldMap.stats.dexterity}\n" +
    $"Mental:       {playerOnWorldMap.stats.mental}\n\n" +
    $"<b>Ship Resources</b>\n" +
    $"Fuel:         {playerOnWorldMap.resources.fuel}\n" +
    $"Rations:      {playerOnWorldMap.resources.food}";

        eventPanel.SetActive(true);

        resolveEvent.GetComponentInChildren<TextMeshProUGUI>().text = "Resolve Event";
        resolveEvent.onClick.RemoveAllListeners();
        resolveEvent.onClick.AddListener(DiceRoll);
    }

    private void DiceRoll() 
    {
        eventPanel.SetActive(false);

        rollcamera.SetActive(true);
        rollButton.gameObject.SetActive(true);

        rollButton.onClick.RemoveAllListeners();
        rollButton.onClick.AddListener(diceRoller.RollTo);
    }

    public void ResolveEvent(int result)
    {
        rollcamera.SetActive(false);

        diceResult.text = "";
        rollButton.gameObject.SetActive(false);

        bool succes = false;
        if (result + EventManager.Instance.GetStatWithModifiers(currentEvent.statType) >= currentEvent.saveDC)
        {
            succes = true; // El evento se resolvió con éxito
        }
        else
        {
            succes = false; // El evento falló
        }

        StartCoroutine(ShowEventResolved(succes));
        currentEvent = null;
    }

    private IEnumerator ShowEventResolved(bool succes)
    {
        eventPanel.SetActive(true);
        if (succes)
        {
            titleText.text = "Éxito: " + currentEvent.eventName;
            descriptionText.text = currentEvent.successText; // o failText según el resultado del evento
        }
        else
        {
            titleText.text = "Fallo: " + currentEvent.eventName;
            descriptionText.text = currentEvent.failText; // o failText según el resultado del evento
        }

        resolveEvent.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        resolveEvent.onClick.RemoveAllListeners();
        resolveEvent.onClick.AddListener(CloseEvent);
        yield return new WaitUntil(() => eventFinished);

        eventFinished = false; // Reiniciar el estado para futuros eventos
        eventPanel.SetActive(false);
        if (onResolvedCallback != null)
        {
            onResolvedCallback.Invoke();
            onResolvedCallback = null; // Limpiar referencia
        }
    }

    private void CloseEvent()
    {
        eventFinished = true;
    }

    public void ShowDiceResult(int targetFace)
    {
        diceResult.gameObject.SetActive(true);
        diceResult.text = "";
        diceResult.text = targetFace.ToString();
    }
}
