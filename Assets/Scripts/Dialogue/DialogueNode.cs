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
        [SerializeField] bool isPlayer = false;

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
        public bool IsPlayer()
        {
            return isPlayer;
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

        public void SetPlayer(bool newIsPlayer)
        {
            Undo.RecordObject(this, "Change Speaker");
            isPlayer = newIsPlayer;
            EditorUtility.SetDirty(this);
        }
#endif
       
    }
}

