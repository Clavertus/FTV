using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureSizeController : MonoBehaviour
{
    [SerializeField] RenderTexture rawImage= null;

    // Update is called once per frame
    void Start()
    {
        int settingResWidth = Screen.width;
        int settingResHeight = Screen.height;
        int pixelatedRate = 2;
        if (PlayerPrefs.HasKey("pixelatedRate"))
        {
            pixelatedRate = PlayerPrefs.GetInt("pixelatedRate");
        }

        if (rawImage != null)
        {
            if(rawImage.width != (settingResWidth / pixelatedRate))
            {
                rawImage.width = settingResWidth / pixelatedRate;
            }
            if (rawImage.height != (settingResHeight / pixelatedRate))
            {
                rawImage.height = settingResHeight / pixelatedRate;
            }
            Debug.Log("Setting game resolution to: " + rawImage.width + "x" + rawImage.height);
        }
    }
}
