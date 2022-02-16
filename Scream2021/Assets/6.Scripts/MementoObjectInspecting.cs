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
    int smallObjInteractionCounter = 0; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        tv = GameObject.Find("TV front");
        
        holdSmallObjCanvas.enabled = false; 
    }

    

     


    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) { StartCoroutine(FirstInteraction()); }
        if (smallObject.CompareTag("Selected") && smallObjInteractionCounter == 0) { StartCoroutine(InspectSmallObject()); }
        
        
        
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
        smallObject.GetComponent<MeshRenderer>().enabled = false;
        holdSmallObjCanvas.enabled = true;
        FindObjectOfType<DialogueUI>().ShowDialogue(smallObjFellDialogue);
        yield return new WaitUntil(() => !DialogueBox.activeSelf);
        
        FindObjectOfType<Examine>().ExitExamineMode(); 
        StartCoroutine(PickedUpObject()); 
    }

    IEnumerator PickedUpObject()
    {

        yield return new WaitUntil(() =>  FindObjectOfType<Examine>().examineMode == false); 

        FindObjectOfType<MouseLook>().UnlockCamera();
        Debug.Log("test");
        
        
        if(symbol) symbol.GetComponent<SymbolInteractions>().IsPocketed(pocketItemName); 
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