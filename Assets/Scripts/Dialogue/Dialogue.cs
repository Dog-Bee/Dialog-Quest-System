using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> nodes = new();
        Dictionary<string, DialogueNode> nodeLookup = new();
        [SerializeField] private Vector2 newNodeOffset = new(250, 0);

        private const float yOffset = 150;

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }


        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildNodes(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();

            foreach (string childID in parentNode.GetChildren())
            {
                if (nodeLookup.TryGetValue(childID, out var value))
                    result.Add(value);
            }

            return result;
        }
#if UNITY_EDITOR

        public void CreateRootNode()
        {
            if (nodes.Count != 0) return;
            nodes.Add(CreateInstance<DialogueNode>());
            nodes[0].name = Guid.NewGuid().ToString();
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            
            AddNode(newNode);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                newNodeOffset.y = yOffset*parent.GetChildren().Count;
                parent.AddChild(newNode.name);
                newNode.SetPlayer(!parent.IsPlayer());
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
            }
            
            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanChildNodes(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }


        private void CleanChildNodes(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}