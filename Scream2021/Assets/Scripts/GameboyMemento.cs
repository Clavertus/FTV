using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboyMemento : MonoBehaviour
{
    [SerializeField] DialogueObject inspectionDialogue;
    [SerializeField] DialogueObject dPadFell;

    [SerializeField] GameObject DialogueBox;
    [SerializeField] GameObject dPad;

    [SerializeField] Canvas dPadCanvas;
    [SerializeField] Canvas heldDPadCanvas;

    [SerializeField] string pocketItem;

    int interactionCounter = 0;
    bool examineMode = false;
    bool pocketedDPad = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = false;
        dPad.gameObject.GetComponent<BoxCollider>().enabled = false;
        heldDPadCanvas.enabled = false; 
    }

    public void ExitedExamineMode() { examineMode = false; Debug.Log("Exited Examine Mode"); }
    public void EnteredExamineMode() { examineMode = true; Debug.Log("Entered Examine Mode"); }
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
        
        FindObjectOfType<DialogueUI>().ShowDialogue(inspectionDialogue);
        
        interactionCounter = 1;
    }

    IEnumerator DPadFell()
    {
        gameObject.tag = ("Untagged");
        GetComponent<BoxCollider>().enabled = false;
        
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = true;
        dPad.gameObject.GetComponent<BoxCollider>().enabled = true; 
        dPadCanvas.gameObject.SetActive(true); 

        gameObject.tag = ("Selectable");
        interactionCounter = 2;
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<DialogueUI>().ShowDialogue(dPadFell);

        yield return new WaitUntil(() => !DialogueBox.activeSelf); 
        StartCoroutine(InspectDPad());
    }

    IEnumerator InspectDPad()
    {
        yield return new WaitUntil(() => examineMode == false);  

        FindObjectOfType<MouseLook>().UnlockCamera();
        Debug.Log("test"); 
        heldDPadCanvas.enabled = true;
        Destroy(dPad);
        GetComponentInParent<SymbolInteractions>().IsPocketed(pocketItem);
         
    }
}
