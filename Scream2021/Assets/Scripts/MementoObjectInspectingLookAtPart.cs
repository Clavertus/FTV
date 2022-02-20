using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoObjectInspectingLookAtPart : MonoBehaviour
{
    [SerializeField] DialogueObject baseObjInspectDialogue;
    [SerializeField] DialogueObject smallObjInspectDialogue;

    //[SerializeField] GameObject DialogueBox;
    [SerializeField] GameObject smallObject;

    [SerializeField] Canvas inspectCanvas;

    GameObject DialogueBox;

    public int interactionCounter = 0;
    public int smallObjInteractionCounter = 0; 
    
    
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
        yield return new WaitUntil(() => !DialogueBox.activeSelf);  
        smallObject.GetComponent<BoxCollider>().enabled = true; 
    }

    public IEnumerator InspectSmallObject() 
    {
        smallObjInteractionCounter++;

        FindObjectOfType<DialogueUI>().ShowDialogue(smallObjInspectDialogue);
        yield return new WaitUntil(() => !DialogueBox.activeSelf);

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

}
