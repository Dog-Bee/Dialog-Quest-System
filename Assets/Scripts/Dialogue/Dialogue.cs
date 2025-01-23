using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes = new(); 
        Dictionary<string,DialogueNode> nodeLookup = new(); 

#if UNITY_EDITOR
        private void Awake()
        {
           OnValidate();
        }
#endif
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public void CreateRootNode()
        {
            nodes.Add(new DialogueNode()
            {
                uniqueID = Guid.NewGuid().ToString(),
            });
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildNodes(DialogueNode parentNode)
        {
            List<DialogueNode> result  = new List<DialogueNode>();

            foreach (string childID in parentNode.children)
            {
                if(nodeLookup.TryGetValue(childID, out var value))
                result.Add(value);
            }
            
            return result;
        }


        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = new DialogueNode();
            newNode.uniqueID = Guid.NewGuid().ToString();
            parent.children.Add(newNode.uniqueID);
            nodes.Add(newNode);
            OnValidate();
        }
    }
}