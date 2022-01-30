using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFpsSetting : MonoBehaviour
{
    [SerializeField] Toggle showFpsToogle = null;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("Setting_ShowFps"))
        {
            bool showFPS = PlayerPrefs.GetInt("Setting_ShowFps") == 1 ? true : false;
            if(showFpsToogle) showFpsToogle.isOn = showFPS;
        }

        if (showFpsToogle) showFpsToogle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        PlayerPrefs.SetInt("Setting_ShowFps", (showFpsToogle.isOn ? 1 : 0));
    }
}
