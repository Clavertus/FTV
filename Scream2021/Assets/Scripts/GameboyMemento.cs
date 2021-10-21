using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboyMemento : MonoBehaviour
{
    [SerializeField] DialogueObject inspectionDialogue;
    [SerializeField] DialogueObject dPadFell;

    [SerializeField] GameObject dPad;
    int interactionCounter = 0;
    bool examineMode = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ExitedExamineMode() { examineMode = false; }
    public void EnteredExamineMode() { examineMode = true; }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && interactionCounter == 0) { FirstInteraction(); }

        if (interactionCounter == 1 && examineMode == false)
        {
            DPadFell();
        }
        
    }

    void FirstInteraction()
    {
        examineMode = true; 
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(inspectionDialogue);
        
        interactionCounter = 1;
    }

    void DPadFell()
    {
        dPad.gameObject.GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = false;
        
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(dPadFell);
        dPad.SetActive(true);
        interactionCounter = 2;
    }
}
