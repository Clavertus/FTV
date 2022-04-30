using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTV.Dialog;
using FTV.Saving;

public class MementoObjectInspectingLookAtPart : MonoBehaviour, ISaveable
{
    [SerializeField] NPCDialogue baseObjInspectDialogue;
    [SerializeField] NPCDialogue smallObjInspectDialogue;
    [SerializeField] bool addExtraTriggerDialog = false;
    [SerializeField] NPCDialogue smallObjTriggerDialogue;
    [SerializeField] GameObject[] revealObjects;

    //[SerializeField] GameObject DialogueBox;
    [SerializeField] GameObject smallObject;

    [SerializeField] Canvas inspectCanvas;

    GameObject DialogueBox;

    public int interactionCounter = 0;
    public int smallObjInteractionCounter = 0;
    public bool smallObjectTrigger = false;
    public bool smallObjectTriggerDone = false;

    PlaySoundOnMementoExamine soundOnMemento = null;

    // Start is called before the first frame update
    void Start()
    {
        DialogueBox = FindObjectOfType<DialogueUI>().dialogueBox;
        soundOnMemento = GetComponentInChildren<PlaySoundOnMementoExamine>();
        if (smallObjInteractionCounter != 0)
        {
            foreach (GameObject obj in revealObjects)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in revealObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) 
        { 
            StartCoroutine(FirstInteraction());
        }
        if (smallObject.CompareTag("Selected") && smallObjInteractionCounter == 0) 
        { 
            StartCoroutine(InspectSmallObject());
        }
        if (gameObject.tag == ("Selected") && (interactionCounter == 1) && (smallObjInteractionCounter > 0))
        {
            interactionCounter++;
            StartCoroutine(SecondInteraction());
        }
    }

    IEnumerator FirstInteraction()
    {
        Debug.Log("select object"); 
        FindObjectOfType<DialogueUI>().ShowDialogue(baseObjInspectDialogue);
        interactionCounter++;
        if (soundOnMemento) soundOnMemento.SetFullSound();
        yield return new WaitUntil(() => !DialogueBox.activeSelf);  
        smallObject.GetComponent<BoxCollider>().enabled = true; 
    }

    public IEnumerator InspectSmallObject() 
    {
        smallObjInteractionCounter++;

        FindObjectOfType<DialogueUI>().ShowDialogue(smallObjInspectDialogue);
        yield return new WaitUntil(() => !DialogueBox.activeSelf);

        //extra objects to show?
        foreach(GameObject obj in revealObjects)
        {
            obj.SetActive(true);
        }

        smallObjectTrigger = true;
        if (addExtraTriggerDialog)
        {
            yield return new WaitUntil(() => smallObjectTriggerDone);

            if (soundOnMemento) soundOnMemento.SetSilent();
            FindObjectOfType<DialogueUI>().ShowDialogue(smallObjTriggerDialogue);
            yield return new WaitUntil(() => !DialogueBox.activeSelf);
        }

        if(soundOnMemento) soundOnMemento.SetSilent();
        GetComponent<ObjectExaminationConfig>().extraPressToShow = false;
        //disable small object collider
        smallObject.GetComponent<BoxCollider>().enabled = false;

        FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        FindObjectOfType<Examine>().ExitExamineMode(); 
        StartCoroutine(ExitInspectionOfThisObject()); 
    }

    IEnumerator ExitInspectionOfThisObject()
    {
        yield return new WaitUntil(() => FindObjectOfType<Examine>().GetExamineMode() == false);

        FindObjectOfType<MouseLook>().UnlockCamera();

        gameObject.tag = ("Selectable");
    }

    IEnumerator SecondInteraction()
    {
        Debug.Log("second interaction");
        FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        FindObjectOfType<Examine>().ExitExamineMode();
        StartCoroutine(ExitInspectionOfThisObject());
        interactionCounter--;
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
        if(smallObjInteractionCounter != 0)
        {
            foreach (GameObject obj in revealObjects)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in revealObjects)
            {
                obj.SetActive(false);
            }
        }
    }
    #endregion

}
