using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookContentHolder : MonoBehaviour
{
    enum page { left, right };

    [SerializeField] CanvasGroup ChangePageUiCanvasLeft = null;
    [SerializeField] CanvasGroup ChangePageUiCanvasRight = null;
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

    int[] currentPageContent = null;

    private void Start()
    {
        ChangePageUiCanvasLeft.alpha = 0;
        ChangePageUiCanvasRight.alpha = 0;

        if (bookContent.Length % 2 != 0)
        {
            Debug.LogError("Book content should be dividable through 2");
            return;
        }
        currentPageContent = new int[2];

        currentPageContent[0] = 0;
        currentPageContent[1] = 1;

        pageImage = new Image[2];
        pageText = new TMP_Text[2];

        pageImage[(int)page.left] = pageLeft.GetComponentInChildren<Image>();
        pageImage[(int)page.right] = pageRight.GetComponentInChildren<Image>();

        pageText[(int)page.left] = pageLeft.GetComponentInChildren<TMP_Text>();
        pageText[(int)page.right] = pageRight.GetComponentInChildren<TMP_Text>();

        FillPageWithContent(page.left, bookContent[0]);
        FillPageWithContent(page.right, bookContent[1]);
    }

    public void ShowUI()
    {
        if(bookContent.Length <= 2)
        {
            return;
        }

        ChangePageUiCanvasRight.alpha = 1;
        ChangePageUiCanvasLeft.alpha = 0;
    }
    public void HideUI()
    {
        ChangePageUiCanvasRight.alpha = 0;
        ChangePageUiCanvasLeft.alpha = 0;
    }

    public void ResetPages()
    {
        int nextIntForLeftPage = 0;
        int nextIntForRightPage = 1;
        FillPageWithContent(page.left, bookContent[nextIntForLeftPage]);
        currentPageContent[(int)page.left] = nextIntForLeftPage;

        FillPageWithContent(page.right, bookContent[nextIntForRightPage]);
        currentPageContent[(int)page.right] = nextIntForRightPage;
    }

    public int GetNumberOfPages()
    {
        return bookContent.Length;
    }

    private void FillPageWithContent(page pageId, pageContent pageContent)
    {
        int pageIntId = (int) pageId;
        pageImage[pageIntId].sprite = pageContent.pageImageSprite;
        pageImage[pageIntId].color = pageContent.pageImageColor;
        pageText[pageIntId].text = pageContent.pageText;
    }

    public void FillPagesWithNextContentRightDirection()
    {
        if (bookContent.Length > 2)
        {
            ChangePageUiCanvasLeft.alpha = 1;
        }
        int nextIntForLeftPage = currentPageContent[(int)page.left] + 2;
        int nextIntForRightPage = currentPageContent[(int)page.right] + 2;

        if (nextIntForLeftPage >= bookContent.Length)
        {
            nextIntForLeftPage = bookContent.Length - 2;
            nextIntForRightPage = bookContent.Length - 1;
            ChangePageUiCanvasRight.alpha = 0;
        }
        else if (nextIntForLeftPage == bookContent.Length -2)
        {
            ChangePageUiCanvasRight.alpha = 0;
        }

        FillPageWithContent(page.left, bookContent[nextIntForLeftPage]);
        currentPageContent[(int)page.left] = nextIntForLeftPage;

        FillPageWithContent(page.right, bookContent[nextIntForRightPage]);
        currentPageContent[(int)page.right] = nextIntForRightPage;
    }
    public void FillPagesWithNextContentLeftDirection()
    {
        if (bookContent.Length > 2)
        {
            ChangePageUiCanvasRight.alpha = 1;
        }
        int nextIntForLeftPage = currentPageContent[(int)page.left] - 2;
        int nextIntForRightPage = currentPageContent[(int)page.right] - 2;

        if (nextIntForLeftPage < 0)
        {
            nextIntForLeftPage = 0;
            nextIntForRightPage = 1;
            ChangePageUiCanvasLeft.alpha = 0;
        }
        else if(nextIntForLeftPage == 0)
        {
            ChangePageUiCanvasLeft.alpha = 0;
        }

        FillPageWithContent(page.left, bookContent[nextIntForLeftPage]);
        currentPageContent[(int)page.left] = nextIntForLeftPage;

        FillPageWithContent(page.right, bookContent[nextIntForRightPage]);
        currentPageContent[(int)page.right] = nextIntForRightPage;
    }
}
