using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Ship,
    Player,
    // Agregá más tipos si hace falta
}

public class EventManager : MonoBehaviour
{

    public static EventManager Instance { get; private set; }
    private bool eventInProgress = false;
    private Dictionary<EventType, IEventInitializer> initializerMap = new();
    private PlayerOnWorldMap playerOnWorldMap;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    IEventInitializer[] initializers = GetComponentsInChildren<IEventInitializer>();
        foreach (var initializer in initializers)
        {
            initializerMap[initializer.GetHandledType()] = initializer;
            Debug.Log("Registered initializer for event type: " + initializer.GetHandledType());
        }

        playerOnWorldMap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerOnWorldMap>();
    }

    public int GetStatWithModifiers(StatType type)
    {
        // add modifiers from the playerOnWorldMap if it exists
        return playerOnWorldMap.GetStat(type);
    }

    public void Trigger(EventSO eventSO)
    {
        if (eventInProgress || eventSO == null) return;

        if (!initializerMap.TryGetValue(eventSO.eventType, out var initializer))
        {
            Debug.LogWarning("No initializer for event type: " + eventSO.eventType);
            return;
        }

        eventInProgress = true;
        Camera.main.GetComponent<CameraController>()?.StopCameraController();

        void FinalizeEvent()
        {
            initializer.TriggerEvent(eventSO);
            EndEvent();
        }

        void StartUI()
        {
            EventUIManager.Instance.ShowEvent(eventSO, FinalizeEvent, playerOnWorldMap);
        }


        if (eventSO.hasDialogue)
        {
            DialogueManager.Instance.StartDialogue(eventSO.dialogue, StartUI);
        }
        else
        {
            StartUI();
        }
    }


    private void EndEvent()
    {
        eventInProgress = false;
        Camera.main.GetComponent<CameraController>()?.StartCameraController();
        // Acá podrías notificar que el juego vuelve al estado libre si hiciera falta
    }
}
