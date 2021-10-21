using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboyMemento : MonoBehaviour
{
    [SerializeField] DialogueObject inspectionDialogue;
    [SerializeField] DialogueObject dPadFell;
    [SerializeField] GameObject DialogueBox; 

    [SerializeField] Canvas dPadCanvas;
    [SerializeField] Canvas heldDPadCanvas;

    [SerializeField] GameObject dPad;
    int interactionCounter = 0;
    bool examineMode = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = false;
        heldDPadCanvas.enabled = false; 
    }

    public void ExitedExamineMode() { examineMode = false; }
    public void EnteredExamineMode() { examineMode = true; }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) { FirstInteraction(); }

        if (interactionCounter == 1 && examineMode == false)
        {
            StartCoroutine(DPadFell());
        }
        
        
    }

    void FirstInteraction()
    {
        examineMode = true; 
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(inspectionDialogue);
        
        interactionCounter = 1;
    }

    IEnumerator DPadFell()
    {
        GetComponent<BoxCollider>().enabled = false;
        
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = true;
        dPadCanvas.gameObject.SetActive(true); 

        gameObject.tag = ("Selectable");
        interactionCounter = 2;
        FindObjectOfType<DialogueUI>().ShowDialogue(dPadFell);

        yield return new WaitUntil(() => !DialogueBox.activeSelf); 
        StartCoroutine(InspectDPad());
    }

    IEnumerator InspectDPad()
    {
        yield return new WaitUntil(() => examineMode == false);  
        
        heldDPadCanvas.enabled = true;
        Destroy(dPad); 
    }
}
