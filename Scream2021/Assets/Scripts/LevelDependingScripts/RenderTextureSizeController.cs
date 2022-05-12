using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureSizeController : MonoBehaviour
{
    int targetResWidth = 640;
    int targetResHeigth = 360;

    [Range(1, 24)]
    [SerializeField] int PixelateRate = 4;

    int width = 1920;
    int heigth = 1080;
    [SerializeField] RenderTexture rawImage= null;

    // Update is called once per frame
    void Start()
    {

        width = Screen.width;
        heigth = Screen.height;

        //targetResWidth = 640;
        /*if (PlayerPrefs.HasKey("resTargetWidth"))
        {
            targetResWidth = PlayerPrefs.GetInt("resTargetWidth");
        }*/
        //targetResHeigth = 360;
        /*if (PlayerPrefs.HasKey("resTargetHeigth"))
        {
            targetResHeigth = PlayerPrefs.GetInt("resTargetHeigth");
        }

        int pixaleRateWidth = width / targetResWidth;
        Debug.LogWarning("pixaleRateWidth: " + pixaleRateWidth);
        int pixaleRateHeigth = heigth / targetResHeigth;
        Debug.LogWarning("pixaleRateHeight: " + pixaleRateHeigth);

        if (pixaleRateWidth == 0) pixaleRateWidth = 1;
        if (pixaleRateHeigth == 0) pixaleRateHeigth = 1;*/


        if (rawImage != null)
        {
            rawImage.width = width / PixelateRate;
            rawImage.height = heigth / PixelateRate;
        }
    }
}
