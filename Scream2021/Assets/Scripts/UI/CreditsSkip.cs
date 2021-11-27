using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSkip : MonoBehaviour
{
    public GameObject skip;

    public float skipTimerCnt = 0f;
    public float skipTimerAppear = 1.5f;

    public Credits credits;

    private void Start()
    {
        skip.SetActive(false);
    }

    private void Update()
    {
        skipTimerCnt += Time.deltaTime;

        if (skipTimerAppear <= skipTimerCnt && !credits.finalPanelLoaded)
        {
            if (LevelLoader.instance.HasPlayedTheGame) skip.SetActive(true);
        }

        if (credits.finalPanelLoaded)
        {
            skip.SetActive(false);
        }

        if (skip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelLoader.instance.HasPlayedTheGame)
                {
                    SkipCredits();
                }
            }
        }
    }

    private void SkipCredits()
    {
        AudioManager.instance.InstantPlayOneShotFromAudioManager(soundsEnum.UIClick);

        credits.StopAllCoroutines();

        foreach (var text in credits.credits)
        {
            StartCoroutine(credits.FadeOutText(text));
        }

        StartCoroutine(credits.FadeInFinalPanel());

        skip.SetActive(false);

        AudioManager.instance.StopFromAudioManager(soundsEnum.Credits);
    }
}
