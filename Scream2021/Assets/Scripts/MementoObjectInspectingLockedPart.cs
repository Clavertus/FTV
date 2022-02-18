using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoObjectInspectingLockedPart : MonoBehaviour
{
    [SerializeField] DialogueObject baseObjInspectDialogue;
    [SerializeField] DialogueObject LockInspectDialogue;

    //[SerializeField] GameObject DialogueBox;
    [SerializeField] GameObject lockObject;

    [SerializeField] Canvas inspectCanvas;

    GameObject DialogueBox;

    int interactionCounter = 0;
    int smallObjInteractionCounter = 0; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        DialogueBox = FindObjectOfType<DialogueUI>().dialogueBox;
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
        if (gameObject.tag == ("Selected") && (interactionCounter > 0) && (smallObjInteractionCounter > 0))
        {
            StartCoroutine(SecondInteraction());
        }
    }

    IEnumerator FirstInteraction()
    {
        Debug.Log("select main object"); 
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

        //disable small object collider
        lockObject.GetComponent<ExamineLockObject>().InspectedOnce();

        FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        FindObjectOfType<Examine>().ExitExamineMode(); 
        StartCoroutine(ExitInspectionOfThisObject()); 
    }

    IEnumerator ExitInspectionOfThisObject()
    {
        yield return new WaitUntil(() => FindObjectOfType<Examine>().examineMode == false);

        FindObjectOfType<MouseLook>().UnlockCamera();

        lockObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.tag = ("Selectable");
        //DisableCanvasAndTriggering();
    }

    private void DisableCanvasAndTriggering()
    {
        inspectCanvas.enabled = false;
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }
    }
    IEnumerator SecondInteraction()
    {
        Debug.Log("second interaction");
        FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(true);

        lockObject.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        FindObjectOfType<Examine>().ExitExamineMode();
        StartCoroutine(ExitInspectionOfThisObject());
    }

}
