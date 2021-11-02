using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEffectController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MaterialSwitcher[] Posters = null;
    [Header("Settings")]
    [SerializeField] public int posterMaterialId = 1;
    int currPosterMaterialId = 0;

    Light[] lightsInTheTrain = null;
    float[] lightsIntensities = null;

    private void Start()
    {
        lightsInTheTrain = GetComponentsInChildren<Light>();
        lightsIntensities = new float[lightsInTheTrain.Length];
        for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
        {
            if (lightsInTheTrain[ix]) lightsIntensities[ix] = lightsInTheTrain[ix].intensity;
        }
    }

    bool flickerLight = false;
    float flickerTime = 0f;
    float flickerTimeCnt = 0f;
    public void FlickerLightForTime(float timeToFlick)
    {
        flickerTimeCnt = 0f;
        flickerTime = timeToFlick;
        for (int ix = 0; ix < lightsInTheTrain.Length; ix ++ )
        {
            if(lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = 0.25f;
        }
        flickerLight = true;
    }

    private void Update()
    {
        HandlePosters();

        if(flickerLight)
        {
            flickerTimeCnt += Time.deltaTime;
            if(flickerTime <= flickerTimeCnt)
            {
                flickerLight = false;
                for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
                {
                    if (lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = lightsIntensities[ix];
                }
            }
        }
    }

    private void HandlePosters()
    {
        if (currPosterMaterialId != posterMaterialId)
        {
            currPosterMaterialId = posterMaterialId;
            changePostersMaterialRuntime(posterMaterialId);
        }
    }

    public void SetPosterMatId(int materialId)
    {
        posterMaterialId = materialId;
    }

    void changePostersMaterialRuntime(int materialId)
    {
        foreach (MaterialSwitcher matSwitch in Posters)
        {
            matSwitch.changeMaterial(materialId);
        }
    }
}
