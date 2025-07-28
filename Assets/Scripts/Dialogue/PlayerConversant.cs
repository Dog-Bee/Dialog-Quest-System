using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;

        private DialogueNode _currentNode = null;

        private void Awake()
        {
            _currentNode = currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if (_currentNode == null)
            {
                return "";
            }

            return _currentNode.GetText();
        }

        public void GetNext()
        {
           DialogueNode[] nodeArray= currentDialogue.GetAllChildNodes(_currentNode).ToArray();
           _currentNode = nodeArray[0];
        }

        public bool HasNext()
        {
            
            return true;
        }
    }
}

