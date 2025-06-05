using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event")]
public class EventSO : ScriptableObject
{
    public string eventName;
    [TextArea(3, 20)]
    public string eventDescription;
    public EventType eventType;

    public int saveDC; // Dificult saving throw
    public Sprite eventIcon; // Event icon
}
