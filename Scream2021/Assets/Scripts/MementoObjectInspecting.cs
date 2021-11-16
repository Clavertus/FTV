using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementoObjectInspecting : MonoBehaviour
{
    [SerializeField] DialogueObject baseObjInspectDialogue;
    [SerializeField] DialogueObject smallObjFellDialogue;

    [SerializeField] GameObject DialogueBox;
    [SerializeField] GameObject smallObject;
    [SerializeField] GameObject symbol; 

    [SerializeField] Canvas inspectCanvas;
    [SerializeField] Canvas holdSmallObjCanvas;

    [SerializeField] string pocketItemName;

    [SerializeField] float distanceFromCam = 2; 

    GameObject tv;

    int interactionCounter = 0;
    bool examineMode = false;
    bool pocketedDPad = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        tv = GameObject.Find("TV front");
        smallObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
        smallObject.gameObject.GetComponent<BoxCollider>().enabled = false;
        holdSmallObjCanvas.enabled = false; 
    }

    public void ExitedExamineMode() { examineMode = false; Debug.Log("Exited Examine Mode"); }
    public void EnteredExamineMode() { examineMode = true; Debug.Log("Entered Examine Mode"); }

    public float ReturnDistanceFromCam() { return distanceFromCam; } 
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) { FirstInteraction(); }

        if (interactionCounter == 1 && examineMode == false)
        {
            StartCoroutine(DPadFell());
            Debug.Log("dropped");
        }
        
        
    }

    void FirstInteraction()
    {
        examineMode = true; 
        
        FindObjectOfType<DialogueUI>().ShowDialogue(baseObjInspectDialogue);
        
        interactionCounter = 1;
    }

    IEnumerator DPadFell()
    {
        gameObject.tag = ("Untagged");
        GetComponent<BoxCollider>().enabled = false;
        
        smallObject.gameObject.GetComponent<MeshRenderer>().enabled = true;
        smallObject.gameObject.GetComponent<BoxCollider>().enabled = true; 
        inspectCanvas.gameObject.SetActive(true); 

        gameObject.tag = ("Selectable");
        interactionCounter = 2;
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<DialogueUI>().ShowDialogue(smallObjFellDialogue);

        yield return new WaitUntil(() => !DialogueBox.activeSelf); 
        StartCoroutine(InspectDPad());
    }

    IEnumerator InspectDPad()
    {
        yield return new WaitUntil(() => examineMode == false);

        FindObjectOfType<MouseLook>().UnlockCamera();
        Debug.Log("test");
        holdSmallObjCanvas.enabled = true;
        Destroy(smallObject);
        symbol.GetComponent<SymbolInteractions>().IsPocketed(pocketItemName); 
        changeTVstatic();

        DisableCanvasAndTriggering();
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

    void changeTVstatic()
    {
        tv.GetComponent<VoidTV>().materialState++;
    }

}
