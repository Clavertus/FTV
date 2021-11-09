using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    bool AudioManagerInitialised = false;

    void MusicPlay()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TV);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    private void OnEnable()
    {
        AudioManagerInitialised = false;
    }

    void Update()
    {
        if((AudioManager.instance) && (!AudioManagerInitialised))
        {
            MusicPlay(); 
            AudioManagerInitialised = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
            AudioManager.instance.StopFromAudioManager(soundsEnum.TV);

            StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
        }
    }
}
