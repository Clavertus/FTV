using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float textFadeSpeed;
    public float textDuration;

    public List<TextMeshProUGUI> credits;

    public CanvasGroup finalPanel;
    public bool finalPanelLoaded;

    public bool delayKeyPress = true;

    void Awake()
    {
        finalPanel.alpha = 0;
        credits = new List<TextMeshProUGUI>();

        foreach (Transform item in gameObject.transform)
        {
            if (item.gameObject.GetComponent<TextMeshProUGUI>())
            {
                TextMeshProUGUI tmp = item.gameObject.GetComponent<TextMeshProUGUI>();
                Color zero = tmp.color;
                zero.a = 0;
                tmp.color = zero;
                credits.Add(item.gameObject.GetComponent<TextMeshProUGUI>());
            }
        }
    }
    void Start()
    {
        StartCoroutine(StartCredits());

        foreach (var sound in AudioManager.instance.sounds)
        {
            sound.source.Stop();
        }

        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Credits);
    }

    void Update()
    {
        if (finalPanelLoaded)
        {
            StartCoroutine(DelayKeyPress());

            if (!delayKeyPress)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    MainManu();
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    QuitGame();
                }
            }
        }
    }

    public void MainManu()
    {
        PlaySound();
        StartCoroutine(LevelLoader.instance.StartLoadingScene(0));
    }

    public void QuitGame()
    {
        PlaySound();
        Application.Quit();
    }

    public void PlaySound()
    {
        AudioManager.instance.InstantPlayOneShotFromAudioManager(soundsEnum.UIClick);
    }

    IEnumerator StartCredits()
    {
        for (int i = 0; i < credits.Count; i++)
        {
            if (i == 0)
            {
                if (LevelLoader.instance.ending == Ending.Unknow)
                {
                    credits[i].text = "UNKNOWN\nENDING";
                }
                else if (LevelLoader.instance.ending == Ending.Bad)
                {
                    credits[i].text = "BAD\nENDING";
                }
                else if (LevelLoader.instance.ending == Ending.Good)
                {
                    credits[i].text = "GOOD\nENDING"; 
                }
            }
            StartCoroutine(FadeInText(credits[i]));

            yield return new WaitForSecondsRealtime(textDuration);

            StartCoroutine(FadeOutText(credits[i]));
        }

        yield return StartCoroutine(FadeInFinalPanel());

        LevelLoader.instance.SetPlayedTheGame();
    }

    public IEnumerator FadeInFinalPanel()
    {
        float t = 0;

        finalPanelLoaded = true;

        while (finalPanel.alpha != 1)
        {
            finalPanel.alpha = Mathf.Lerp(finalPanel.alpha, 1, t);
            t += textFadeSpeed * Time.deltaTime;

            yield return null;
        }
    }
    public IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        StopCoroutine(FadeInText(text));

        Color temp = text.color;
        float t = 0;

        while (text.color.a != 0)
        {
            temp.a = Mathf.Lerp(temp.a, 0, t);
            t += textFadeSpeed * Time.deltaTime;
            text.color = temp;

            yield return null;
        }
    }

    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        StopCoroutine(FadeOutText(text));

        Color temp = text.color;
        float t = 0;

        while (text.color.a != 1)
        {
            temp.a = Mathf.Lerp(temp.a, 1, t);
            t += textFadeSpeed * Time.deltaTime;
            text.color = temp;

            yield return null;
        }
    }

    IEnumerator DelayKeyPress()
    {
        yield return new WaitForSecondsRealtime(1);

        delayKeyPress = false;
    }
}
