using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCDialog : MonoBehaviour
{
    public Action DialogNodeIsStarted { get; set; }
    public Action DialogNodeIsEnded { get; set; }
    public Action DialogIsFinished { get; set; }

    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] NPCLookAtPlayer npc = null;
    [SerializeField] NPCAnimationController npcAnimator = null;

    int interactionCounter = 0;

    private DialogueUI dialogUI = null;

    private void Awake()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        dialogUI.OnDialogNodeStart += playTalkAnimation;
        dialogUI.OnDialogNodeStart += NPCDialogNodeStarted;
        dialogUI.OnDialogNodeEnd += NPCDialogNodeEnd;
        dialogUI.OnDialogShowEnd += playIdleAnimation;
        dialogUI.OnDialogShowEnd += NPCDialogFinished;
    }

    private void NPCDialogNodeStarted(bool isPlayerSpeaking)
    {
        if(!isPlayerSpeaking)
        {
            DialogNodeIsStarted?.Invoke();
        }
    }
    private void NPCDialogNodeEnd()
    {
        DialogNodeIsEnded?.Invoke();
    }
    private void NPCDialogFinished()
    {
        DialogIsFinished?.Invoke();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        Debug.Log("Play NPC Dialogue");
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

    public void SetNewDialogAvailableNoPlay(FTV.Dialog.NPCDialogue newDialogObject)
    {
        dialogObject = newDialogObject;

        gameObject.tag = "Selectable";
        interactionCounter = 0;
    }
    public void SetNewDialogAvailableAndPlay(FTV.Dialog.NPCDialogue newDialogObject)
    {
        dialogObject = newDialogObject;
        Interaction();
    }
}
