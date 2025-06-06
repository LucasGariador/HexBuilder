using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.MPE;
using UnityEngine;

namespace Atropos.Dialogue.Editor
{
    /// <summary>
    /// Dialogue Editor Window
    /// </summary>
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized]
        GUIStyle nodeStyle;
        [NonSerialized]
        GUIStyle playerNodeStyle;
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;
        const float canvasSize = 4000;
        const float backgroundSize = 115.5f;

        Vector2 scrollPosition;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            Debug.Log("Editor Created");
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }
        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChange()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI() 
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
            }
            else
            {
                ProcessEvent();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backGroundTex = Resources.Load("texture_08") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize/backgroundSize, canvasSize/backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backGroundTex, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                EditorGUILayout.EndScrollView();
                if(creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }



        private void ProcessEvent()
        {
            if(Event.current.type == UnityEngine.EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if(Event.current.type == UnityEngine.EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

                GUI.changed = true;
            }else if (Event.current.type == UnityEngine.EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if(Event.current.type == UnityEngine.EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if(Event.current.type == UnityEngine.EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }



        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;
            style = node.IsPlayerSpeaking() ? playerNodeStyle : nodeStyle;

            GUILayout.BeginArea(node.GetRect(), style);
            EditorGUI.BeginChangeCheck();
            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                creatingNode = node;
            }
            if (GUILayout.Button("Delete"))
            {
                deletingNode = node;
            }
            GUILayout.EndHorizontal();
            DrawLinkButtons(node);

            //foreach (DialogueNode childrenNode in selectedDialogue.GetAllChildren(node))
            //{
            //    EditorGUILayout.LabelField(childrenNode.text);
            //}
            EditorGUI.EndChangeCheck();
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else
            {
                if (linkingParentNode.name == node.name)
                {
                    if (GUILayout.Button("Cancel"))
                    {
                        linkingParentNode = null;
                    }
                }
                else if (linkingParentNode.GetChildren().Contains(node.name))
                {
                    if (GUILayout.Button("Unlink"))
                    {

                        linkingParentNode.RemoveChildren(node.name);
                        linkingParentNode = null;
                    }
                }
                else
                {
                    if (GUILayout.Button("Child"))
                    {
                        linkingParentNode.AddChildren(node.name);
                        linkingParentNode = null;
                    }
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = (new Vector2(node.GetRect().center.x + node.GetRect().width / 2, node.GetRect().center.y));
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = (new Vector2(childNode.GetRect().center.x - childNode.GetRect().width/2 , childNode.GetRect().center.y));
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach(DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
} 
