public interface IEventInitializer
{
    EventType GetHandledType();
    void TriggerEvent(EventSO eventSO);
}
