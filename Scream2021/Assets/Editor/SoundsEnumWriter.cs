using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioManager))]
public class SoundsEnumWriter : Editor
{
    AudioManager audioManager;
    string filePath = "Assets/Scripts/Audio/";
    string fileName = "soundsEnum";

    private void OnEnable()
    {
        //creating an object of type AudioManager for esier access 
        audioManager = (AudioManager)target;
    }

    public override void OnInspectorGUI()
    {
        //Making a custom Inspector
        base.OnInspectorGUI();

        List<string> names = new List<string>();

        //Getting just the names of the Sound objects
        foreach (var item in audioManager.sounds)
        {
            names.Add(item.name);
        }

        //Creating a button and adding functionallyty to it
        if (GUILayout.Button("Save"))
        {
            EditorMethods.WriteToEnum(filePath, fileName, names);
        }
    }
}
