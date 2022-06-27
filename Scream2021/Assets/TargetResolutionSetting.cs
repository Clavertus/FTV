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

        if (PlayerPrefs.HasKey("pixelatedRate") == false)
        {
            if (ResolutionSettings[0])
            {
                SetResolutionIdToTrue(0);
                ValueChange(0);
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("pixelatedRate") == 2)
            {
                SetResolutionIdToTrue(0);
                ValueChange(0);
            }
            else if(PlayerPrefs.GetInt("pixelatedRate") == 4)
            {
                SetResolutionIdToTrue(1);
                ValueChange(1);
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
                PlayerPrefs.SetInt("pixelatedRate", 2);
            }
            else if(id == 1)
            {
                PlayerPrefs.SetInt("pixelatedRate", 4);
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
