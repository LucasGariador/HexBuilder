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
    }

    public void Trigger(EventSO eventSO)
    {
        if (eventInProgress)
        {
            Debug.LogWarning("Already resolving an event.");
            return;
        }

        if (eventSO == null)
        {
            Debug.LogError("Event is null.");
            return;
        }

        if (!initializerMap.TryGetValue(eventSO.eventType, out var initializer))
        {
            Debug.LogWarning("No initializer for event type: " + eventSO.eventType);
            return;
        }

        eventInProgress = true;

        // 1. Detener movimiento de cámara y entrada
        Camera.main.GetComponent<CameraController>()?.StopCameraController();

        // 2. Mostrar la UI
        EventUIManager.Instance.ShowEvent(eventSO, () =>
        {
            // Callback cuando el usuario termina de ver la UI o de tirar el dado
            initializer.TriggerEvent(eventSO);
            EndEvent();
        });
    }

    private void EndEvent()
    {
        eventInProgress = false;
        Camera.main.GetComponent<CameraController>()?.StartCameraController();
        // Acá podrías notificar que el juego vuelve al estado libre si hiciera falta
    }
}
