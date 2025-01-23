using System;
using System.Collections.Specialized;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue selectedDialogue = null;
        private GUIStyle nodeStyle;
        private DialogueNode draggingNode = null;
        private Vector2 draggingOffset;

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
            NodeStyleInit();
        }

        private void NodeStyleInit()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OpenDialogue(int dialogueID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(dialogueID) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnSelectionChange()
        {
            Dialogue tempDialogue = Selection.activeObject as Dialogue;
            if (tempDialogue != null)
            {
                selectedDialogue = tempDialogue;
                Repaint();
            }
        }


        private void OnGUI()
        {
            if (selectedDialogue != null)
            {
                DraggingNode();
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawBezierConnection(node);
                    GUIDrawNode(node);
                }
            }
        }

        private void DrawBezierConnection(DialogueNode node)
        {
            Vector3 startPos = new Vector2(node.rect.xMax, node.rect.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildNodes(node))
            {
                Vector3 endPos = new Vector2(childNode.rect.xMin, childNode.rect.center.y);

                Vector3 controlPointOffset = endPos - startPos;
                controlPointOffset.y = 0;
                controlPointOffset.x *= .8f;
                Handles.DrawBezier(startPos, endPos, startPos + controlPointOffset, endPos - controlPointOffset,
                    Color.white, null, 4f);
            }
        }

        private void GUIDrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField($"Node:", EditorStyles.boldLabel);
            string newUniqueID = EditorGUILayout.TextField(node.uniqueID);
            string newText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Edit Dialogue Text");
                node.uniqueID = newUniqueID;
                node.text = newText;
            }

            GUILayout.EndArea();
        }

        private void DraggingNode()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
                }
            }

            if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Drag Dialogue");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }

            if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 currentMousePosition)
        {
            DialogueNode currentNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(currentMousePosition))
                {
                    currentNode = node;
                }
            }

            return currentNode;
        }
    }
}