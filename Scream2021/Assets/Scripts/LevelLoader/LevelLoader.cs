using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public CanvasGroup canvasGroup;
    public float fadeStep;
    public float fadeTime;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeOut());
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public IEnumerator StartLoadingNextScene()
    {
        StopAllCoroutines();
        yield return StartCoroutine(FadeIn());
        LoadNextScene();
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FadeOut());
    }

    public IEnumerator StartLoadingScene(int index)
    {
        StopAllCoroutines();
        yield return StartCoroutine(FadeIn());
        LoadScene(index);
        yield return StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeStep;
            yield return new WaitForSeconds(fadeTime);
        }
    }

    public IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeStep;
            yield return new WaitForSeconds(fadeTime);
        }
    }
}
