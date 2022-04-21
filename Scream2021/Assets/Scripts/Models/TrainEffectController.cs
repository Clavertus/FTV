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
    [SerializeField] MeshRenderer[] Lamps = null;
    [SerializeField] Material[] trainMaterials = null;
    [SerializeField] MeshRenderer[] trainBaseToChange = null;

    private soundsEnum lampsOff;
    private soundsEnum lampsOn;

    private Color LampsBasicColor = Color.white;

    private void Start()
    {
        lampsOff = soundsEnum.LampsSuddenlyOff;
        lampsOn = soundsEnum.LampsGetOn;

        if (Lamps.Length > 0)
        {
            Lamps[0].sharedMaterials[1].SetColor("_EmissionColor", LampsBasicColor);
        }

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
    [SerializeField] float minLightIntensityByFlick = 0.5f;
    public void FlickerLightForTime(float timeToFlick)
    {
        flickerTimeCnt = 0f;
        flickerTime = timeToFlick;
        for (int ix = 0; ix < lightsInTheTrain.Length; ix ++ )
        {
            if(lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = minLightIntensityByFlick;
        }

        flickerLight = true;
    }
    public IEnumerator StopLightFlick()
    {
        flickerTimeCnt = 0f;
        flickerLight = false;

        for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
        {
            if (lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = lightsIntensities[ix];
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.05f));
        }
    }

    bool light_is_on = true;
    public IEnumerator SetLightOff()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(lampsOff);
        yield return new WaitForSeconds(0.3f);
        light_is_on = false;
        flickerTimeCnt = 0f;
        for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
        {
            if (lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = 0.0f;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.05f));
        }

        if (Lamps.Length > 0)
        {
            Lamps[0].sharedMaterials[1].SetColor("_EmissionColor", Color.black);
        }
    }
    public IEnumerator SetLightOn()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(lampsOn);
        yield return new WaitForSeconds(0.45f);
        light_is_on = true;
        flickerTimeCnt = 0f;

        if (Lamps.Length > 0)
        {
            Lamps[0].sharedMaterials[1].SetColor("_EmissionColor", LampsBasicColor);
        }

        for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
        {
            if (lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = lightsIntensities[ix];
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.15f));
        }
    }

    private void Update()
    {
        HandlePosters();

        if (light_is_on)
        {
            HandleFlickLight();
        }
    }

    void HandleFlickLight()
    {
        if (flickerLight)
        {
            flickerTimeCnt += Time.deltaTime;
            if (flickerTime <= flickerTimeCnt)
            {
                flickerLight = false;
                for (int ix = 0; ix < lightsInTheTrain.Length; ix++)
                {
                    if (lightsInTheTrain[ix]) lightsInTheTrain[ix].intensity = lightsIntensities[ix];
                }

                if (Lamps.Length > 0)
                {
                    Lamps[0].sharedMaterials[1].SetColor("_EmissionColor", LampsBasicColor);
                }
            }
        }
    }

    void HandlePosters()
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

    public enum trainMaterialType{ normal, rusty00, rusty01 };
    public void setTrainMaterial(trainMaterialType trainMaterial)
    {
        int materialId = (int)trainMaterial;
        Debug.Log("DEBUG: " + materialId);

        if (materialId > trainMaterials.Length)
        {
            return;
        }

        foreach(MeshRenderer renderer in trainBaseToChange)
        {
            renderer.sharedMaterial = trainMaterials[materialId];
            Debug.Log(renderer + "set material to " + trainMaterials[materialId]);
        }

        Debug.Log("DEBUG: " + trainMaterials[materialId]);
    }
}
