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
    public bool HasPlayedTheGame;
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

        if(PlayerPrefs.HasKey("HasPlayedTheGame"))
        {
            HasPlayedTheGame = PlayerPrefs.GetInt("HasPlayedTheGame") == 1 ? true : false;
        }
    }

    public void SetPlayedTheGame()
    {
        HasPlayedTheGame = true;
        PlayerPrefs.SetInt("HasPlayedTheGame", HasPlayedTheGame ? 1 : 0);
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
        float t = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeStep;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, t);
            t += fadeStep * Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeStep;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, t);
            t += fadeStep * Time.deltaTime;
            yield return new WaitForSeconds(fadeTime);
        }
    }
}
