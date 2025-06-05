using UnityEngine;
using Atropos.Dialogue;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace Atropos.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] GameObject iaResponse;
        [SerializeField] float typingSpeed = 0.1f;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] GameObject dialoguePanel;

        [SerializeField] PortraitEffect playerPortrait;
        [SerializeField] PortraitEffect aiPortrait;

        private bool finishedTyping = false;
        //private bool endedDialogue = false;
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.OnConversationUpdated += UpdateUi;
            dialoguePanel.SetActive(false);
        }

        private void Update()
        {
            if (playerConversant == null || !playerConversant.HasStarted() || playerConversant.IsChoosing()) return;

            if (!finishedTyping) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (playerConversant.HasNext())
                {
                    playerConversant.Next();
                }
                else
                {
                    playerConversant.EndConversation(); // Método que podés crear para encapsular lógica
                }
            }
        }




        private void UpdateUi()
        {
            if (!playerConversant.IsActive() || playerConversant.HasEnded())
            {
                dialoguePanel.SetActive(false);
                return;
            }

            if (dialoguePanel.activeSelf == false) 
            {
            dialoguePanel.SetActive(true);
            }

            iaResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                TypewriterEffect();
            }
        }

        private void SwitchBetweenSpeaker()
        {

        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choiseNode in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = choiseNode.GetText();
                choiceInstance.GetComponentInChildren<Button>().onClick.AddListener(() => 
                {
                    Debug.Log("Clicked option");
                    playerConversant.SelectChoice(choiseNode);
                });
            }
        }
        private void TypewriterEffect()
        {
            finishedTyping = false;
            StartCoroutine(TypeText());
        }
        IEnumerator TypeText()
        {
            AIText.text = "";
            foreach (char letter in playerConversant.GetText().ToCharArray())
            {
                AIText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            Debug.Log("Done Writing");
            finishedTyping = true;
        }
    }
}