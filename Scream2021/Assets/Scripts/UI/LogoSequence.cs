using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogoSequence : MonoBehaviour
{
    [SerializeField] float silentDelayAtStart = 5f;
    [SerializeField] float silentDelayAtEnd = 5f;
    public float textFadeSpeed;
    public float textDuration;

    [SerializeField] List<CanvasGroup> imagelist;

    public bool playSound = true;
    public soundsEnum sound = soundsEnum.Credits;

    void Awake()
    {
        /*imagelist = new List<Image>();

        foreach (Transform item in gameObject.transform)
        {
            if (item.gameObject.GetComponent<Image>())
            {
                Image tmp = item.gameObject.GetComponent<Image>();
                Color zero = tmp.color;
                zero.a = 0;
                tmp.color = zero;
                imagelist.Add(item.gameObject.GetComponent<Image>());
            }
        }*/
    }

    IEnumerator Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioManager.instance.StopAllSounds();
        foreach(CanvasGroup img in imagelist)
        {
            img.alpha = 0;
        }

        yield return new WaitForSeconds(silentDelayAtStart);

        StartCoroutine(StartCredits());

        if(playSound) AudioManager.instance.PlayOneShotFromAudioManager(sound);
    }

    IEnumerator StartCredits()
    {
        for (int i = 0; i < imagelist.Count; i++)
        {
            StartCoroutine(FadeInImage(imagelist[i]));

            yield return new WaitForSecondsRealtime(textDuration);

            StartCoroutine(FadeOutImage(imagelist[i]));
        }

        if (playSound) AudioManager.instance.StopFromAudioManager(sound);
        yield return new WaitForSeconds(silentDelayAtEnd);
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }

    public IEnumerator FadeOutImage(CanvasGroup img)
    {
        StopCoroutine(FadeInImage(img));

        float tmp_alpha = img.alpha;
        float t = 0;

        while (img.alpha != 0)
        {
            tmp_alpha = Mathf.Lerp(tmp_alpha, 0, t);
            t += textFadeSpeed * Time.deltaTime;
            img.alpha = tmp_alpha;

            yield return null;
        }
    }

    IEnumerator FadeInImage(CanvasGroup img)
    {
        StopCoroutine(FadeOutImage(img));

        float tmp_alpha = img.alpha;
        float t = 0;

        while (img.alpha != 1)
        {
            tmp_alpha = Mathf.Lerp(tmp_alpha, 1, t);
            t += textFadeSpeed * Time.deltaTime;
            img.alpha = tmp_alpha;

            yield return null;
        }
    }

}
