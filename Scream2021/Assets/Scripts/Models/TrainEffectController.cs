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

    private void Start()
    {
        //TODO: find another place for this function call
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    private void Update()
    {
        HandlePosters();
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
