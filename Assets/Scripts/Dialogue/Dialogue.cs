using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes; 
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
    }
}