using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour { 

    void Start()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TV);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
            AudioManager.instance.StopFromAudioManager(soundsEnum.TV);

            StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
        }
    }
}
