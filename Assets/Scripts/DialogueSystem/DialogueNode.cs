using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Atropos.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false;
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> children = new();
        [SerializeField]
        private Rect rect = new Rect(0,0,200,100);

         public Rect GetRect() { return rect; }

        public string GetText() { return text; }

        public List<string> GetChildren() {  return children; }

        public bool IsPlayerSpeaking() {  return isPlayerSpeaking; }

#if UNITY_EDITOR


        public void SetPlayerIsSpeaking(bool value)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isPlayerSpeaking = value;
            EditorUtility.SetDirty(this);
        }
        public void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPos;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "UpdateDialogue");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChildren(string childID)
        {
            Undo.RecordObject(this, "Add Node Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChildren(string childID)
        {
            Undo.RecordObject(this, "Remove Node Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
