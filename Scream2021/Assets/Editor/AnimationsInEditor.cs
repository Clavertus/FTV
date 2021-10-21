using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PassengerAnimation))]
public class AnimationsInEditor : Editor
{
    PassengerAnimation passAnim;
    private void OnEnable()
    {
        passAnim = (PassengerAnimation)target;
    }
    public override void OnInspectorGUI()
    {
        //Making a custom Inspector
        base.OnInspectorGUI();
        GUILayout.Label("Editing: " + (passAnim.isInEditorMode ? "TRUE" : "FALSE"));
        if (GUILayout.Button("Enter Preview") && !passAnim.isInEditorMode)
        {
            AnimationMode.StartAnimationMode();
            GUILayout.Toggle(AnimationMode.InAnimationMode(), "Animate", EditorStyles.toolbarButton);
            foreach (var item in FindObjectsOfType<PassengerAnimation>())
            {
                item.isInEditorMode = true;
            }
        }
        if (GUILayout.Button("Exit Preview") && passAnim.isInEditorMode)
        {
            Debug.LogError("TFF");
            AnimationMode.EndSampling();
            AnimationMode.StopAnimationMode();
            foreach (var item in FindObjectsOfType<PassengerAnimation>())
            {
                item.isInEditorMode = false;
            }
        }
    } 
}