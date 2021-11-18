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

    
    GameObject tv;

    int interactionCounter = 0;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        tv = GameObject.Find("TV front");
        smallObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
        smallObject.gameObject.GetComponent<BoxCollider>().enabled = false;
        holdSmallObjCanvas.enabled = false; 
    }

    

     


    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) { FirstInteraction(); }

        if (interactionCounter == 1 && FindObjectOfType<Examine>().examineMode == false )  
        {
            
            StartCoroutine(DPadFell());
            Debug.Log("dropped");
        }
        
        
    }

    void FirstInteraction()
    { 

        Debug.Log("select object"); 
        FindObjectOfType<DialogueUI>().ShowDialogue(baseObjInspectDialogue);
        
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
        yield return new WaitUntil(() =>  FindObjectOfType<Examine>().examineMode == false); 

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
