using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayFromAudioManager(soundsEnum.TV);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.StopFromAudioManager(soundsEnum.TV);
            StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
        }
    }
}
