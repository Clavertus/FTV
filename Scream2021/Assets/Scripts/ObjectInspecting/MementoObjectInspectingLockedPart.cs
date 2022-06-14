using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTV.Dialog;
using FTV.Saving;

public class MementoObjectInspectingLockedPart : MonoBehaviour, ISaveable
{
    [Header("Animation")]
    [SerializeField] Animator mainObjectAnimator = null;
    [SerializeField] string preOpenAnimationTriggerName = "Pre-Open";
    [SerializeField] string openAnimationTriggerName = "Open";
    [SerializeField] string CloseAnimationTriggerName = "Close";

    [Header("Dialogs")]
    [SerializeField] NPCDialogue baseObjInspectDialogue;
    [SerializeField] NPCDialogue LockInspectDialogue;
    [SerializeField] NPCDialogue UnlockedDialogue;

    [Header("References")]
    [SerializeField] GameObject lockObject;
    ExamineLockObject examineLockObject = null;
    [SerializeField] Canvas inspectCanvas;

    GameObject DialogueBox;
    ExamineCanvas examineCanvas = null;

    int interactionCounter = 0;
    int smallObjInteractionCounter = 0; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        examineCanvas = FindObjectOfType<ExamineCanvas>();
        DialogueBox = FindObjectOfType<DialogueUI>().dialogueBox;
        examineLockObject = lockObject.GetComponent<ExamineLockObject>();
        examineLockObject.Unlocked += OnUnlock;
    }

    private void OnUnlock()
    {
        lockObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(StartOpenInteraction());
    }

    private IEnumerator StartOpenInteraction()
    {
        StartCoroutine(StartOpenAnimation());
        StopCoroutine(SecondInteraction());
        interactionCounter--;

        FindObjectOfType<DialogueUI>().ShowDialogue(UnlockedDialogue);
        yield return new WaitUntil(() => !DialogueBox.activeSelf);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        FindObjectOfType<Examine>().ExitExamineMode();
        StartCoroutine(ExitInspectionOfThisObject());
    }

    private IEnumerator StartOpenAnimation()
    {
        mainObjectAnimator.SetTrigger(preOpenAnimationTriggerName);
        yield return new WaitForSeconds(0.2f);
        mainObjectAnimator.SetTrigger(openAnimationTriggerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) 
        { 
            StartCoroutine(FirstInteraction());
        }
        if (lockObject.CompareTag("Selected") && smallObjInteractionCounter == 0) 
        { 
            StartCoroutine(InspectLock());
        }
        if (gameObject.tag == ("Selected") && (interactionCounter == 1) && (smallObjInteractionCounter > 0))
        {
            interactionCounter++;
            StartCoroutine(SecondInteraction());
        }
    }

    IEnumerator FirstInteraction()
    {
        //Debug.Log("select main object"); 
        FindObjectOfType<DialogueUI>().ShowDialogue(baseObjInspectDialogue);
        interactionCounter++;
        yield return new WaitUntil(() => !DialogueBox.activeSelf);  

        lockObject.GetComponent<BoxCollider>().enabled = true;
    }

    public IEnumerator InspectLock() 
    {
        smallObjInteractionCounter++;

        FindObjectOfType<DialogueUI>().ShowDialogue(LockInspectDialogue);
        yield return new WaitUntil(() => !DialogueBox.activeSelf);

        GetComponent<ObjectExaminationConfig>().extraPressToShow = false;
        examineLockObject.InspectedOnce();
        examineLockObject.EnterInspection();

        examineCanvas.SetExtraFieldToState(true);

        bool exit = false;
        while(exit == false)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
            if (examineLockObject.IsUnlocked() == false)
            {
                if(examineLockObject.IsRotating() == false)
                {
                    exit = true;
                }
            }
            else
            {
                exit = true;
            }
        }

        examineLockObject.ExitInspection();

        if (examineLockObject.IsUnlocked() == false)
        {
            FindObjectOfType<Examine>().ExitExamineMode();
            StartCoroutine(ExitInspectionOfThisObject());
        }
        else if((examineLockObject.IsUnlocked() == true))
        {
            yield return new WaitUntil(() => !DialogueBox.activeSelf);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
            FindObjectOfType<Examine>().ExitExamineMode();
            StartCoroutine(ExitInspectionOfThisObject());
        }
    }

    IEnumerator ExitInspectionOfThisObject()
    {
        yield return new WaitUntil(() => FindObjectOfType<Examine>().GetExamineMode() == false);

        if (examineLockObject.IsUnlocked() == true)
        {
            mainObjectAnimator.SetTrigger(CloseAnimationTriggerName);
        }

        FindObjectOfType<MouseLook>().UnlockCamera();

        lockObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.tag = ("Selectable");
    }

    IEnumerator SecondInteraction()
    {
        //Debug.Log("second interaction");

        if(examineLockObject.IsUnlocked() == true)
        {
            StartCoroutine(StartOpenAnimation());
        }
        else
        {
            examineLockObject.EnterInspection();
            lockObject.GetComponent<BoxCollider>().enabled = true;
        }

        yield return new WaitForSeconds(0.1f);

        examineCanvas.SetExtraFieldToState(true);

        bool exit = false;
        while (exit == false)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));
            if (examineLockObject.IsUnlocked() == false)
            {
                if (examineLockObject.IsRotating() == false)
                {
                    exit = true;
                }
            }
            else
            {
                exit = true;
            }
        }
        examineLockObject.ExitInspection();

        interactionCounter--;
        FindObjectOfType<Examine>().ExitExamineMode();
        StartCoroutine(ExitInspectionOfThisObject());
    }


    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
        public int smallObjInteractionCounter;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        data.smallObjInteractionCounter = smallObjInteractionCounter;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;
        smallObjInteractionCounter = data.smallObjInteractionCounter;
    }
    #endregion

}
