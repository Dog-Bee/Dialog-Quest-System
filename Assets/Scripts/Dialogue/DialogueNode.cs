using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public List<string> children;
        public Rect rect = new Rect(0,0,200,100);
    }
}

