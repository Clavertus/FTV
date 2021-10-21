using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraChanger : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject train;
    public CanvasGroup image;

    public float fadeSpeed = 0.01f;
    public float duration = 3f;
    private void Start()
    {
        StartCoroutine(ManageTransitions());
    }

    IEnumerator ManageTransitions()
    {
        StartCoroutine(FadeOut());
        cam1.SetActive(true);
        cam2.SetActive(false);
        train.SetActive(false);
        FindObjectOfType<AudioManager>().PlayFromAudioManager(soundsEnum.Change4);
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        StartCoroutine(FadeInWithTrain());
    }

    IEnumerator FadeInWithTrain()
    {
        while (image.alpha < 1)
        {
            image.alpha += fadeSpeed;
            yield return new WaitForSeconds(fadeSpeed);
        }
        cam1.SetActive(false);
        cam2.SetActive(true);
        train.SetActive(true);
        StartCoroutine(FadeOut());
        FindObjectOfType<AudioManager>().PlayFromAudioManager(soundsEnum.Change7);
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        while (image.alpha < 1)
        {
            image.alpha += fadeSpeed;
            yield return new WaitForSeconds(fadeSpeed);
        }
        //transiton to next scene
    }

    IEnumerator FadeOut()
    {
        while (image.alpha > 0)
        {
            image.alpha -= 0.001f;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
