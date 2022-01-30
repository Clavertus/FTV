using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogOnInspection : MonoBehaviour
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] bool disableSameDialogues = true;

    PlayDialogOnInspection[] listOfObjects = null;
    int interactionCounter = 0;

    private DialogueUI dialogUI = null;
    private void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }

        if(interactionCounter > 0)
        {
            if (!dialogUI.dialogueBox.activeSelf) 
            {
                Debug.Log("DisableAllSameObjects of: " + name);
                DisableAllSameObjects();
                gameObject.tag = ("Untagged");
                this.GetComponent<Selectable>().enabled = false;
                this.enabled = false;
            }
        }
    }

    private void Interaction()
    {
        listOfObjects = FindObjectsOfType<PlayDialogOnInspection>();

        interactionCounter++;
        if (dialogUI)
        {
            if(dialogObject)
            {
                dialogUI.ShowDialogue(dialogObject);
            }
        }
    }

    public void DisableAllSameObjects()
    {
        foreach(PlayDialogOnInspection obj in listOfObjects)
        {
            if (obj == this) continue;

            if(obj.dialogObject == this.dialogObject)
            {
                obj.gameObject.tag = ("Untagged");
                obj.GetComponent<Selectable>().enabled = false;
                obj.enabled = false;
            }
        }
    }
}
