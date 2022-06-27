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
        if (ResolutionSettings[2]) ResolutionSettings[2].onValueChanged.AddListener(delegate { ValueChange( 2 ); });
        if (ResolutionSettings[3]) ResolutionSettings[3].onValueChanged.AddListener(delegate { ValueChange( 3 ); });

        if (PlayerPrefs.HasKey("resTargetWidth") == false)
        {
            if (ResolutionSettings[0])
            {
                SetResolutionIdToTrue(0);
                ValueChange(0);
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("resTargetWidth") == 1280)
            {
                SetResolutionIdToTrue(0);
                ValueChange(0);
            }
            else if(PlayerPrefs.GetInt("resTargetWidth") == 960)
            {
                SetResolutionIdToTrue(1);
                ValueChange(1);
            }
            else if (PlayerPrefs.GetInt("resTargetWidth") == 720)
            {
                SetResolutionIdToTrue(2);
                ValueChange(2);
            }
            else
            {
                SetResolutionIdToTrue(3);
                ValueChange(3);
            }
        }
    }

    private void SetResolutionIdToTrue(int id)
    {
        SetOthersToFalse(id);
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
            }
            else if(id == 1)
            {
                PlayerPrefs.SetInt("resTargetWidth", 960);
                PlayerPrefs.SetInt("resTargetHeigth", 540);
            }
            else if (id == 2)
            {
                PlayerPrefs.SetInt("resTargetWidth", 720);
                PlayerPrefs.SetInt("resTargetHeigth", 480);
            }
            else
            {
                PlayerPrefs.SetInt("resTargetWidth", 640);
                PlayerPrefs.SetInt("resTargetHeigth", 360);
            }
            SetOthersToFalse(id);
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

    private void SetOthersToFalse(int id)
    {
        for(int ix = 0; ix < ResolutionSettings.Length; ix++)
        {
            if (ix == id) continue;
            ResolutionSettings[ix].isOn = false;
        }
    }
}
