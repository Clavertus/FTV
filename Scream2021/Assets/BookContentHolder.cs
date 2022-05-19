using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookContentHolder : MonoBehaviour
{
    enum page { left, right };

    [SerializeField] CanvasGroup pageLeft = null;
    [SerializeField] CanvasGroup pageRight = null;

    [System.Serializable]
    public struct pageContent
    {
        public string pageText;
        public Sprite pageImageSprite;
        public Color pageImageColor;
    };


    [SerializeField] pageContent[] bookContent = null;

    Image[] pageImage = null;

    TMP_Text[] pageText = null;

    private void Start()
    {
        pageImage = new Image[2];
        pageText = new TMP_Text[2];

        pageImage[(int)page.left] = pageLeft.GetComponentInChildren<Image>();
        pageImage[(int)page.right] = pageRight.GetComponentInChildren<Image>();

        pageText[(int)page.left] = pageLeft.GetComponentInChildren<TMP_Text>();
        pageText[(int)page.right] = pageRight.GetComponentInChildren<TMP_Text>();

        FillPageWithContent(page.left, bookContent[0]);
        FillPageWithContent(page.right, bookContent[1]);
    }

    private void FillPageWithContent(page pageId, pageContent pageContent)
    {
        int pageIntId = (int) pageId;
        pageImage[pageIntId].sprite = pageContent.pageImageSprite;
        pageImage[pageIntId].color = pageContent.pageImageColor;
        pageText[pageIntId].text = pageContent.pageText;
    }
}
