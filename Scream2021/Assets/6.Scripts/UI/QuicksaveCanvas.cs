using System;
using UnityEngine;

public class QuicksaveCanvas : MonoBehaviour
{
    public static QuicksaveCanvas instance;
    private CanvasGroup cg;

    void Awake()
    {
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

        cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    public void StartAnimation() // QuicksaveCanvas.instance.StartAnimation()
    {
        Action action = new Action(FadeOut);

        LeanTween.alphaCanvas(cg, 1f, 0.5f).setEaseInCirc();
        LeanTween.alphaCanvas(cg, 0f, 1f).setLoopPingPong(2).setDelay(2f).setOnComplete(action);
    }

    void FadeOut()
    {
        LeanTween.alphaCanvas(cg, 0f, 1f).setEaseInCirc();
    }
}
