using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimate : MonoBehaviour
{
    [SerializeField] Texture[] texture_sequence = null;
    [SerializeField] float[] time_control = null;
    [SerializeField] bool pingPongAnimate = true;
    [SerializeField] bool applyOnEmmisionMap = false;
    [SerializeField] bool useExtraTimeModifier = true;
    [SerializeField] float extraTimeModifier = 2f;

    int currentTexture = 0;
    float timeCount = 0f;
    SkinnedMeshRenderer skinnedMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        currentTexture = 0;
        timeCount = 0f;
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if(!skinnedMeshRenderer)
        {
            Debug.LogError("No skinned mesh renderer found on object");
            return;
        }
        if (time_control.Length != texture_sequence.Length)
        {
            Debug.LogError("time and texture should have the same length");
            return;
        }
        if(texture_sequence.Length <= 1)
        {
            Debug.LogError("this script makes sense only when there is at least 2 textures!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextureSwap();
    }

    private bool toogle = false;
    private void TextureSwap()
    {
        float timeToCheck = time_control[currentTexture];
        if (useExtraTimeModifier)
        {
            timeToCheck = time_control[currentTexture] * extraTimeModifier;
        }
        if (timeCount > timeToCheck)
        {
            timeCount = 0;

            if(pingPongAnimate)
            {
                if(toogle == false)
                {
                    currentTexture++;
                    if (currentTexture >= time_control.Length)
                    {
                        toogle = true;
                        currentTexture = time_control.Length - 2;
                    }
                }
                else
                {
                    currentTexture--;
                    if (currentTexture == -1)
                    {
                        toogle = false;
                        currentTexture = 1;
                    }
                }
            }
            else
            {
                currentTexture++;
                if (currentTexture >= time_control.Length)
                {
                    currentTexture = 0;
                }
            }

            if (applyOnEmmisionMap)
            {
                skinnedMeshRenderer.material.SetTexture("_EmissionMap", texture_sequence[currentTexture]);
            }
            else
            {
                skinnedMeshRenderer.material.mainTexture = texture_sequence[currentTexture];
            }
        }
        else
        {
            timeCount += Time.deltaTime;
        }
    }
}
