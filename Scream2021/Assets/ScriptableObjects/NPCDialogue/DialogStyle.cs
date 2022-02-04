using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FTV.Dialog
{
    [CreateAssetMenu(fileName = "New dialogue style", menuName = "ScriptableObjects/DialogStyle", order = 1)]
    [System.Serializable]
    public class DialogStyle : ScriptableObject
    {
        [SerializeField]
        Sprite image = null;
        [SerializeField]
        Color color;

        public Sprite GetImage()
        {
            return image;
        }
        public Color GetColor()
        {
            return color;
        }
    }
}
