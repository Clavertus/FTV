using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    bool AudioManagerInitialised = false;

    void MusicPlay()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.TV);
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Drone);
    }

    private void Awake()
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
