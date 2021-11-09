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

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    public void TriggerSecondDroneSound()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone2);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (AudioManager.instance)
        {
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone2);
        }
    }
}
