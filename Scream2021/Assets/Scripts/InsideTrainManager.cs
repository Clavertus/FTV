using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideTrainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.instance.PlayFromAudioManager(soundsEnum.Drone);
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
    }

    public void TriggerSecondDroneSound()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.PlayFromAudioManager(soundsEnum.Drone2);
    }

    private void OnDisable()
    {
        if(AudioManager.instance)
        {
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone2);
        }
    }
}
