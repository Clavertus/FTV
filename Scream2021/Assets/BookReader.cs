using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookReader : MonoBehaviour
{
    [SerializeField] Animator bookAnimator = null;
    [SerializeField] BookContentHolder bookContent = null;

    private int interactionCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactionCounter = 0;
        bookAnimator.SetTrigger("Closed");
    }

    private void Update()
    {
        if (gameObject.tag == "Selected" && (interactionCounter == 0)) { StartCoroutine(Interaction()); return; }
    }

    private IEnumerator Interaction()
    {
        //gameObject.tag = "Untagged";
        interactionCounter++;

        FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(true);
        FindObjectOfType<PlayerMovement>().LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();

        OpenBook();

        bool exit = false;
        while (exit == false)
        {
            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)));
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    exit = true;
                }
                else if(Input.GetKeyDown(KeyCode.A))
                {
                    ListBookLeft(); yield return new WaitForSeconds(0.1f);
                }
                else if(Input.GetKeyDown(KeyCode.D))
                {
                    ListBookRight(); yield return new WaitForSeconds(0.1f);
                }
            }
        }


        FindObjectOfType<PlayerMovement>().UnlockPlayer();
        FindObjectOfType<MouseLook>().UnlockCamera();
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();

        CloseBook();

        interactionCounter--;
        gameObject.tag = ("Selectable");
        FindObjectOfType<Examine>().ExitExamineMode();
    }

    public void OpenBook()
    {
        bookAnimator.SetTrigger("Open");
        bookContent.ResetPages();
        bookContent.ShowUI();
    }
    public void CloseBook()
    {
        bookAnimator.SetTrigger("Close");
        bookContent.HideUI();
    }

    public void ListBookRight()
    {
        bookContent.FillPagesWithNextContentRightDirection();
    }
    public void ListBookLeft()
    {
        bookContent.FillPagesWithNextContentLeftDirection();
    }
}
