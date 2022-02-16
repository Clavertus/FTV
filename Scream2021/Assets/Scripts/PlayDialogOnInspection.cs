using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogOnInspection : MonoBehaviour, ISaveable
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] bool disableSameDialogues = true;

    PlayDialogOnInspection[] listOfObjects = null;

    public int interactionCounter = 0;

    private DialogueUI dialogUI = null;

    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }

        if(interactionCounter == 1)
        {
            if (!dialogUI.dialogueBox.activeSelf) 
            {
                Debug.Log("DisableAllSameObjects of: " + name);
                DisableAllSameObjects();
                gameObject.tag = ("Untagged");
                this.GetComponent<Selectable>().enabled = false;
                interactionCounter++;
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
        foreach(PlayDialogOnInspection inspectableObj in listOfObjects)
        {
            if (inspectableObj == this) continue;

            if(inspectableObj.dialogObject == this.dialogObject)
            {
                inspectableObj.SetAsUsed();
            }
        }
    }

    public void SetAsUsed()
    {
        gameObject.tag = ("Untagged");
        this.GetComponent<Selectable>().enabled = false;
        interactionCounter = 2;
    }

    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;

        if(interactionCounter >= 1)
        {
            SetAsUsed();
        }
    }
}
