using System;
using System.Collections.Specialized;
using Codice.Client.Common.TreeGrouper;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        [NonSerialized] private Dialogue selectedDialogue = null;
        [NonSerialized] private GUIStyle nodeStyle;
        [NonSerialized] private DialogueNode draggingNode = null;
        [NonSerialized] private Vector2 draggingOffset;

        [NonSerialized] private DialogueNode creatingNode = null;
        [NonSerialized] private DialogueNode deletingNode = null;
        [NonSerialized] private DialogueNode linkingNode = null;
        
        [NonSerialized] private Vector2 scrollPosition;
        [NonSerialized] private bool draggingWindow = false;
        [NonSerialized] private Vector2 draggingWindowOffset;


        private const float windowSize = 4000;
        private const float graphSize = 50;

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
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect window = GUILayoutUtility.GetRect(windowSize, windowSize);
                Texture2D graphTexture = Resources.Load("Editor/graph") as Texture2D;
                Rect textureCoordinates = new Rect(0, 0, windowSize/graphSize, windowSize/graphSize);
                GUI.DrawTextureWithTexCoords(window,graphTexture,textureCoordinates);
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawBezierConnection(node);
                    GUIDrawNode(node);
                }
                EditorGUILayout.EndScrollView();
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }

                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }

        private void DrawBezierConnection(DialogueNode node)
        {
            Vector3 startPos = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildNodes(node))
            {
                Vector3 endPos = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);

                Vector3 controlPointOffset = endPos - startPos;
                controlPointOffset.y = 0;
                controlPointOffset.x *= .8f;
                Handles.DrawBezier(startPos, endPos, startPos + controlPointOffset, endPos - controlPointOffset,
                    Color.white, null, 4f);
            }
        }

        private void GUIDrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.GetRect(), nodeStyle);
            EditorGUILayout.LabelField($"Node:", EditorStyles.boldLabel);
            node.SetText(EditorGUILayout.TextField(node.GetText()));

            DrawGUIButtons(node);

            GUILayout.EndArea();
        }

        private void DrawGUIButtons(DialogueNode node)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }

            GUILayout.EndHorizontal();
            DrawGUILinkingButtons(node);
        }

        private void DrawGUILinkingButtons(DialogueNode node)
        {
            if (linkingNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingNode = node;
                }
            }
            else if (linkingNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingNode = null;
                } 
            }
            else if (linkingNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Disconnect"))
                {
                    linkingNode.RemoveChild(node.name);
                    linkingNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Connect"))
                {
                    linkingNode.AddChild(node.name);
                    linkingNode = null;
                } 
            }
        }

        private void DraggingNode()
        {

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (draggingNode == null)
                    {
                        draggingNode = GetNodeAtPoint(Event.current.mousePosition+scrollPosition);
                        if (draggingNode != null)
                        {
                            draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                            Selection.activeObject = draggingNode;
                        }
                        else
                        {
                            draggingWindow = true;
                            draggingWindowOffset = Event.current.mousePosition + scrollPosition;
                            Selection.activeObject = selectedDialogue;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (draggingNode != null)
                    {
                        draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                        GUI.changed = true;
                        return;
                    }

                    if (draggingWindow)
                    {
                        scrollPosition = draggingWindowOffset - Event.current.mousePosition;
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseUp:
                    if (draggingNode != null)
                    {
                        draggingNode = null;
                    }

                    if (draggingWindow)
                    {
                        draggingWindow = false;
                    }
                    break;
            }
           
        }

        private DialogueNode GetNodeAtPoint(Vector2 currentMousePosition)
        {
            DialogueNode currentNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(currentMousePosition))
                {
                    currentNode = node;
                }
            }

            return currentNode;
        }
    }
}