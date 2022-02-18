using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureSizeController : MonoBehaviour
{

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

        if (rawImage != null)
        {
            rawImage.width = width / PixelateRate;
            rawImage.height = heigth / PixelateRate;
        }
    }
}
