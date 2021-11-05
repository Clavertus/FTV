using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    bool AudioManagerInitialised = false;
    // Start is called before the first frame update
    void MusicPlay()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TV);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    private void OnEnable()
    {
        AudioManagerInitialised = false;
    }

    // Update is called once per frame
    void Update()
    {
        if((AudioManager.instance) && (!AudioManagerInitialised))
        {
            MusicPlay(); 
            AudioManagerInitialised = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
        }
    }
}
