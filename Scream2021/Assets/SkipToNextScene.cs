using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToNextScene : MonoBehaviour
{
    [SerializeField] GameObject skip;
    [SerializeField] soundsEnum[] soundsToStop = null;

    private void Start()
    {
        skip.SetActive(false);
    }


    float skipTimerCnt = 0f;
    float skipTimerAppear = 1.5f;
    // Update is called once per frame
    void Update()
    {
        skipTimerCnt += Time.deltaTime;
        if (skipTimerAppear <= skipTimerCnt)
        {
            if (LevelLoader.instance.HasPlayedTheGame) 
                skip.SetActive(true);
        }

        if (skip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelLoader.instance.HasPlayedTheGame) 
                    SkipAndLoadNextScene();
            }
        }
    }

    private void SkipAndLoadNextScene()
    {
        foreach(soundsEnum sound in soundsToStop)
        {
            AudioManager.instance.StopFromAudioManager(sound);
        }

        GetPlayerPrefsFromScene();

        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }

    private void GetPlayerPrefsFromScene()
    {
        GameObject player = null;
        GameObject cam = null;

        player = GameObject.Find("Player");
        if(player) cam = player.transform.GetChild(0).gameObject;

        if(player && cam)
        {
            float playerYrot = player.transform.rotation.eulerAngles.y;
            float camXrot = cam.transform.rotation.eulerAngles.x;

            PlayerPrefs.SetFloat("playerYrot", playerYrot);
            PlayerPrefs.SetFloat("camXrot", camXrot);
        }
    }

}
