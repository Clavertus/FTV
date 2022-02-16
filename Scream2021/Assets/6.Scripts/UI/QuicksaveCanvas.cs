using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuicksaveCanvas : MonoBehaviour
{
    public static QuicksaveCanvas instance;
    private bool fadingOut;
    private CanvasGroup cg;

    void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>();

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

    void OnEnable()
    {
        cg.alpha = 0;

        LeanTween.alphaCanvas(cg, 1f, 1f).setEaseInCirc();
        LeanTween.alphaCanvas(cg, 0f, 1f).setLoopPingPong().setDelay(2f);
    }

    void Update()
    {
        if (!LeanTween.isTweening(gameObject) && fadingOut)
        {
            gameObject.SetActive(false);
        }
    }

    public void FadeOut()
    {
        LeanTween.alphaCanvas(cg, 0f, 1f).setEaseInCirc();
        fadingOut = true;
    }
}
