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
    public Button rollButton;

    private System.Action onResolvedCallback;
    private EventSO currentEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        eventPanel.SetActive(false);
    }

    public void ShowEvent(EventSO eventSO, System.Action onResolved)
    {
        onResolvedCallback = onResolved;

        currentEvent = eventSO;
        titleText.text = eventSO.eventName;
        descriptionText.text = eventSO.eventDescription;
        eventPanel.SetActive(true);

        rollButton.onClick.RemoveAllListeners();
        rollButton.onClick.AddListener(ResolveEvent);
    }

    private void ResolveEvent()
    {
        eventPanel.SetActive(false);
        currentEvent = null;

        if (onResolvedCallback != null)
        {
            onResolvedCallback.Invoke();
            onResolvedCallback = null; // Limpiar referencia
        }
    }
}
