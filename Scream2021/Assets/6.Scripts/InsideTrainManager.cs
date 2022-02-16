using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideTrainManager : MonoBehaviour
{
    // called zero
    void Awake()
    {
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
    }

    void Start()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    public void TriggerSecondDroneSound()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone2);
    }
}
