using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasShowOnEnable : MonoBehaviour
{
    [SerializeField] CanvasGroup canvas = null;

    bool show = false;
    private void OnEnable()
    {
        show = true;
        canvas.alpha = 0f;
    }

    private void Update()
    {
        if (show == false) return;

        if(canvas.alpha < 1.0f) canvas.alpha += 1.0f * Time.deltaTime;
    }
}
