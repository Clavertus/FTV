using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetResolutionSetting : MonoBehaviour
{
    [SerializeField] Toggle[] ResolutionSettings = null;

    // Start is called before the first frame update
    void Start()
    {
        if (ResolutionSettings[0]) ResolutionSettings[0].onValueChanged.AddListener(delegate { ValueChange( 0 ); });
        if (ResolutionSettings[1]) ResolutionSettings[1].onValueChanged.AddListener(delegate { ValueChange( 1 ); });

        if (PlayerPrefs.HasKey("resTargetWidth") == false)
        {
            if (ResolutionSettings[0])
            {
                SetResoultionIdToTrue(0);
                PlayerPrefs.SetInt("resTargetWidth", 1280);
                PlayerPrefs.SetInt("resTargetHeigth", 720);
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("resTargetWidth") == 1280)
            {
                SetResoultionIdToTrue(0);
            }
            else
            {
                SetResoultionIdToTrue(1);
            }
        }
    }

    private void SetResoultionIdToTrue(int id)
    {
        foreach(Toggle toogle in ResolutionSettings)
        {
            toogle.isOn = false;
        }
        ResolutionSettings[id].isOn = true;
    }

    public void ValueChange(int id)
    {
        if(ResolutionSettings[id].isOn)
        {
            if (id == 0)
            {
                PlayerPrefs.SetInt("resTargetWidth", 1280);
                PlayerPrefs.SetInt("resTargetHeigth", 720);
                ResolutionSettings[1].isOn = false;
            }
            else
            {
                PlayerPrefs.SetInt("resTargetWidth", 640);
                PlayerPrefs.SetInt("resTargetHeigth", 360);
                ResolutionSettings[0].isOn = false;
            }
        }
        else
        {
            bool checkIfAllWasDisabled = true;
            foreach(Toggle toogle in ResolutionSettings)
            {
                if(toogle.isOn == true)
                {
                    checkIfAllWasDisabled = false;
                }
            }
            if(checkIfAllWasDisabled == true)
            {
                ResolutionSettings[id].isOn = true;
            }
        }
    }
}
