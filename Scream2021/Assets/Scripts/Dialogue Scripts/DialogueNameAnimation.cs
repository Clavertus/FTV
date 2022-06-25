using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNameAnimation : MonoBehaviour
{
    public float lerpDuration = 3;
    public float startValue = 0;
    private float endValue = 60;
    public float valueToLerp;

    private float timeElapsed;
    private CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        valueToLerp = Mathf.PingPong(Time.time / 2, 1);
        timeElapsed += Time.deltaTime;
        cg.alpha = (float)(valueToLerp + 0.2);
    }
}
