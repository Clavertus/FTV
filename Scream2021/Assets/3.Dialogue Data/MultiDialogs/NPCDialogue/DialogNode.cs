using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FTV.Dialog
{
    [System.Serializable]
    public class DialogNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false; //turn to enum in case of more participants then 2
        [SerializeField] 
        string text;
        [SerializeField]
        string speakerName = "";
        [SerializeField] 
        List<string> children = new List<string>();
        [SerializeField]
        int triggerId = 0;
        [SerializeField] 
        Rect rect = new Rect(0,0,300,130);
        [SerializeField]
        DialogStyle style;


        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }
        public string GetSpeakerName()
        {
            return speakerName;
        }

        public List<string> GetChildren()
        {
            return children;
        }
        public bool GetIsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }
        public DialogStyle GetDialogStyle()
        {
            return style;
        }
        public int GetTriggerId()
        {
            return triggerId;
        }
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move dialog node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Node");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void SetSpeakerName(string newSpeakerName)
        {
            if (newSpeakerName != speakerName)
            {
                Undo.RecordObject(this, "Update Dialogue Node");
                speakerName = newSpeakerName;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChildren(string newChildren)
        {
            Undo.RecordObject(this, "Link dialogue child");
            children.Add(newChildren);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChildren(string childrenToRemove)
        {
            Undo.RecordObject(this, "Unlink dialogue child");
            children.Remove(childrenToRemove);
            EditorUtility.SetDirty(this);
        }
        public void SetIsPlayerSpeaking(bool value)
        {
            Undo.RecordObject(this, "Edit dialogue node");
            isPlayerSpeaking = value;
            EditorUtility.SetDirty(this);
        }
        public void SetDialogStyle(DialogStyle newStyle)
        {
            if((newStyle != style) && (newStyle != null))
            {
                Undo.RecordObject(this, "Edit dialogue node");
                style = newStyle;
                EditorUtility.SetDirty(this);
            }
        }
        public void SetTriggerId(int newId)
        {
            triggerId = newId;
            Undo.RecordObject(this, "Edit dialogue node");
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
