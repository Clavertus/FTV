using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNPCDialog : MonoBehaviour
{
    public Action PreTriggerEventCall { get; set; }
    public Action DialogIsStarted { get; set; }
    public Action<int> DialogNodeIsStarted { get; set; }
    public Action<int> DialogNodeIsEnded { get; set; }
    public Action DialogIsFinished { get; set; }

    [SerializeField] FTV.Dialog.NPCDialogue dialogObject = null;
    [SerializeField] NPCLookAtPlayer npc = null;
    [SerializeField] NPCAnimationController npcAnimator = null;

    int interactionCounter = 0;
    bool preTriggerEvent = false;
    private DialogueUI dialogUI = null;

    private void Awake()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    private void NPCDialogIsStarted(FTV.Dialog.NPCDialogue f_dialogObject)
    {
        if (f_dialogObject != dialogObject) return;
        DialogIsStarted?.Invoke();
    }

    private void NPCDialogNodeStarted(FTV.Dialog.NPCDialogue f_dialogObject, bool isPlayerSpeaking, int TriggerId)
    {
        if (f_dialogObject != dialogObject) return;
        if (!isPlayerSpeaking)
        {
            Debug.Log("Dialog with id: " + TriggerId);
            DialogNodeIsStarted?.Invoke(TriggerId);
        }
    }
    private void NPCDialogNodeEnd(FTV.Dialog.NPCDialogue f_dialogObject, int TriggerId)
    {
        Debug.Log("Dialog with id: " + TriggerId);
        if (f_dialogObject != dialogObject) return;
        DialogNodeIsEnded?.Invoke(TriggerId);
    }
    private void NPCDialogFinished(FTV.Dialog.NPCDialogue f_dialogObject)
    {
        if (f_dialogObject != dialogObject) return;
        DialogIsFinished?.Invoke();

        UnsubscribeOnDialogEvents();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        if (dialogUI)
        {
            if (dialogObject)
            {
                if(preTriggerEvent)
                {
                    PreTriggerEventCall?.Invoke();
                    DisableInteraction();
                }
                else
                {
                    //subscribe on dialogUI
                    SubscribeOnDialogEvents();

                    FindObjectOfType<MouseLook>().LockAndLookAtPoint(npc.GetLookAtPoint().position);
                    dialogUI.ShowDialogue(dialogObject);
                    DisableInteraction();
                }
            }
        }
    }

    public void PreTriggerEventFinished()
    {
        SubscribeOnDialogEvents();

        FindObjectOfType<MouseLook>().LockAndLookAtPoint(npc.GetLookAtPoint().position);
        dialogUI.ShowDialogue(dialogObject);
        preTriggerEvent = false;
    }

    private void SubscribeOnDialogEvents()
    {
        dialogUI.OnDialogShowStart += NPCDialogIsStarted;
        dialogUI.OnDialogNodeStart += playTalkAnimation;
        dialogUI.OnDialogNodeStart += NPCDialogNodeStarted;
        dialogUI.OnDialogNodeEnd += NPCDialogNodeEnd;
        dialogUI.OnDialogShowEnd += playIdleAnimation;
        dialogUI.OnDialogShowEnd += NPCDialogFinished;
    }
    private void UnsubscribeOnDialogEvents()
    {
        dialogUI.OnDialogShowStart -= NPCDialogIsStarted;
        dialogUI.OnDialogNodeStart -= playTalkAnimation;
        dialogUI.OnDialogNodeStart -= NPCDialogNodeStarted;
        dialogUI.OnDialogNodeEnd -= NPCDialogNodeEnd;
        dialogUI.OnDialogShowEnd -= playIdleAnimation;
        dialogUI.OnDialogShowEnd -= NPCDialogFinished;
    }

    private void playIdleAnimation(FTV.Dialog.NPCDialogue f_DialogObject)
    {
        if (f_DialogObject != dialogObject) return;
        if (npcAnimator.GetCurrentState() != NPCAnimationController.NpcAnimationState.sit)
        {
            npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.idle);
        }
    }

    public bool awryState = false;
    private void playTalkAnimation(FTV.Dialog.NPCDialogue f_DialogObject, bool isPlayerSpeaking, int TriggerId)
    {
        if (f_DialogObject != dialogObject) return;
        if (npcAnimator.GetCurrentState() != NPCAnimationController.NpcAnimationState.sit)
        {
            if (!isPlayerSpeaking)
            {
                if(awryState == true)
                {
                    int randomTalkId = UnityEngine.Random.Range((int)0, (int)3);
                    npcAnimator.SetAnimation((NPCAnimationController.NpcAnimationState) ((int)NPCAnimationController.NpcAnimationState.awry0 + randomTalkId));
                }
                else
                {
                    npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.talk);
                }
            }
            else
            {
                if (awryState == true)
                {
                    npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.awryIdle);
                }
                else
                {
                    npcAnimator.SetAnimation(NPCAnimationController.NpcAnimationState.idle);
                }
            }
        }
    }

    public void DisableInteraction()
    {
        gameObject.tag = "Untagged";
        interactionCounter++;
        if(GetComponent<Selectable>()) GetComponent<Selectable>().enabled = false;
    }
    public void SetNewDialogAvailableNoPlayAddPreTrigger(FTV.Dialog.NPCDialogue newDialogObject)
    {
        dialogObject = newDialogObject;
        gameObject.tag = "Selectable";
        interactionCounter = 0;
        preTriggerEvent = true;
    }

    public void SetNewDialogAvailableNoPlay(FTV.Dialog.NPCDialogue newDialogObject)
    {
        dialogObject = newDialogObject;
        gameObject.tag = "Selectable";
        interactionCounter = 0;
        preTriggerEvent = false;
    }

    public void SetNewDialogAvailableAndPlay(FTV.Dialog.NPCDialogue newDialogObject)
    {
        dialogObject = newDialogObject;
        gameObject.tag = "Selectable";
        interactionCounter = 0;
        preTriggerEvent = false;
        // play now
        Interaction();
    }
}
