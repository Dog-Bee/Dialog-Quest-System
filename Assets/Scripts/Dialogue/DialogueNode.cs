using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Dialogue
{
    [Serializable]
    public class DialogueNode :ScriptableObject
    {
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new();
        [SerializeField] private Rect rect = new Rect(0,0,200,125);

        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildren()
        {
            return children;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Drag Dialogue");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if(newText == text) return;
            
            Undo.RecordObject(this, "Edit Dialogue Text");
            text = newText;
            EditorUtility.SetDirty(this);

        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Connect Dialogue");
            children.Add(childID);
            EditorUtility.SetDirty(this);

        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Disconnect Dialogue");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}

