using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCDialog : MonoBehaviour
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] NPCLookAtPlayer npc = null;
    [SerializeField] NPCAnimationController npcAnimator = null;

    int interactionCounter = 0;

    private DialogueUI dialogUI = null;
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        dialogUI.OnDialogShowStart += playTalkAnimation;
        dialogUI.OnDialogShowEnd += playIdleAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        gameObject.tag = "Untagged";

        interactionCounter++;
        if (dialogUI)
        {
            if (dialogObject)
            {
                FindObjectOfType<MouseLook>().LockAndLookAtPoint(npc.GetLookAtPoint().position);
                dialogUI.ShowDialogue(dialogObject);
                GetComponent<Selectable>().enabled = false;
            }
        }
    }

    private void playIdleAnimation()
    {
        npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.idle);
    }

    private void playTalkAnimation(bool isPlayerSpeaking)
    {
        if (!isPlayerSpeaking)
        {
            npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.talk);
        }
        else
        {
            npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.idle);
        }
    }
}
