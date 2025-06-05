using Atropos.Dialogue;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private PlayerConversant playerConversant;
    [SerializeField] private float delayBeforeEnd = 2f; // Tiempo de espera antes de finalizar el diálogo

    private void Awake()
    {
        Instance = this;
        playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
    }

    public void StartDialogue(Dialogue dialogue, System.Action onDialogueFinished)
    {
        playerConversant.StartDialogue(dialogue);

        StartCoroutine(RunDialogue(dialogue, onDialogueFinished));
    }

    private IEnumerator RunDialogue(Dialogue dialogue, System.Action onFinished)
    {
        yield return new WaitUntil(() => playerConversant.HasEnded());
        yield return new WaitForSeconds(delayBeforeEnd); // Espera un segundo antes de finalizar
        onFinished?.Invoke();
    }

}
