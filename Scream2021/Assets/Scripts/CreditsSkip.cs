using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSkip : MonoBehaviour
{
    [SerializeField] GameObject Skip;

    private void Start()
    {
        Skip.SetActive(false);
    }

    float skipTimerCnt = 0f;
    float skipTimerAppear = 1.5f;
    // Update is called once per frame
    void Update()
    {
        skipTimerCnt += Time.deltaTime;
        if (skipTimerAppear <= skipTimerCnt)
        {
            if(LevelLoader.instance.HasPlayedTheGame) Skip.SetActive(true);
        }

        if(Skip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (LevelLoader.instance.HasPlayedTheGame) SkipScene(); 
            }
        }
    }

    private void SkipScene()
    {
        AudioManager.instance.PauseFromAudioManager(soundsEnum.Credits);
        StartCoroutine(LevelLoader.instance.StartLoadingScene(0));
    }
}
