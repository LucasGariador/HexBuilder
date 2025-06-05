using Atropos.Dialogue;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(Dialogue dialogue, Action onDialogueFinished)
    {
        // Mostrar di�logo con tu sistema
        // Al finalizar:
        StartCoroutine(RunDialogue(dialogue, onDialogueFinished));
    }

    private IEnumerator RunDialogue(Dialogue dialogue, Action onFinished)
    {
        // Reproduc�s cada l�nea, esper�s input del jugador...

        yield return new WaitUntil(() => true);

        onFinished?.Invoke();
    }

}
