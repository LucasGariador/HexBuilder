using UnityEngine;

public class ShipEventInitializer : MonoBehaviour, IEventInitializer
{
    public EventType GetHandledType() => EventType.Ship;

    public void TriggerEvent(EventSO eventSO)
    {
        Debug.Log("Ship event triggered: " + eventSO.eventName);
        // Lógica del evento
    }
}