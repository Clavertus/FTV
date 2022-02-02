using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCDialog : MonoBehaviour
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] NPCLookAtPlayer npc = null;

    int interactionCounter = 0;

    private DialogueUI dialogUI = null;
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        interactionCounter++;
        if (dialogUI)
        {
            if (dialogObject)
            {
                FindObjectOfType<MouseLook>().LockAndLookAtPoint(npc.GetLookAtPoint().position);
                dialogUI.ShowDialogue(dialogObject);
            }
        }
    }
}
