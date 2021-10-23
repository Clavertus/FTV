using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Ending
{
    Unknow,
    Bad,
    Good
}
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public CanvasGroup canvasGroup;
    public float fadeStep;
    public float fadeTime;

    public Ending ending;
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
        DontDestroyOnLoad(gameObject);
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
        StartCoroutine(FadeOut());
    }

    public IEnumerator StartLoadingScene(int index)
    {
        StopAllCoroutines();
        yield return StartCoroutine(FadeIn());
        LoadScene(index);
        StartCoroutine(FadeOut());
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
