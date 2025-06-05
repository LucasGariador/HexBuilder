using Atropos.Dialogue;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event")]
public class EventSO : ScriptableObject
{
    public string eventName;
    [TextArea(3, 20)]
    public string eventDescription;
    public EventType eventType;
    public bool hasDialogue; // the event has dialogue associated with it
    public Dialogue dialogue; // the dialogue associated with the event
    [TextArea(3, 20)]
    public string successText; // Text to show on success
    [TextArea(3, 20)]
    public string failText; // Text to show on failure

    public StatType statType; // Type of stat to check
    public int saveDC; // Dificult saving throw
    public Sprite eventIcon; // Event icon
}
