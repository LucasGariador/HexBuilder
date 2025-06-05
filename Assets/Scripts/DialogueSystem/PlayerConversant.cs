using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atropos.Dialogue
{

    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        bool hasStarted = false;

        public event System.Action OnConversationUpdated;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialogue(currentDialogue);
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            if(currentDialogue == null) return;

            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            isChoosing = false;
            hasStarted = true;
            OnConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool HasStarted()
        {
            return hasStarted;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }
            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
             return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                OnConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAiChildren(currentNode).ToArray();
            int randomIndex = Random.Range(0, children.Count());
            currentNode = children[randomIndex];

            OnConversationUpdated();
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }
    }
}