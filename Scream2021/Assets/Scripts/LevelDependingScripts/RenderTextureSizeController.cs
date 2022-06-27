using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureSizeController : MonoBehaviour
{
    int targetResWidth = 1280;
    int targetResHeigth = 720;

    //[Range(1, 24)]
    //[SerializeField] int PixelateRate = 4;

    [SerializeField] RenderTexture rawImage= null;

    // Update is called once per frame
    void Start()
    {
        int settingResWidth = targetResWidth;
        int settingResHeigth = targetResHeigth;
        if (PlayerPrefs.HasKey("resTargetWidth"))
        {
            settingResWidth = PlayerPrefs.GetInt("resTargetWidth");
        }
        if (PlayerPrefs.HasKey("resTargetHeigth"))
        {
            settingResHeigth = PlayerPrefs.GetInt("resTargetHeigth");
        }

        if (rawImage != null)
        {
            rawImage.width = settingResWidth;
            rawImage.height = settingResHeigth;
        }
    }
}
