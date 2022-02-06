using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tara_Behaviour : MonoBehaviour
{
    [Header("Dialogs:")]
    [SerializeField] FTV.Dialog.NPCDialogue dialog_0 = null;
    bool dialog_0_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_1 = null;
    bool dialog_1_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_2 = null;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_3 = null;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_4 = null;


    [Header("References:")]
    [SerializeField] PlayNPCDialog npc_Dialog = null;

    [Header("Location Points:")]
    [SerializeField] Transform point_0 = null;
    [SerializeField] Transform point_1 = null;
    [SerializeField] Transform point_2 = null;

    int behaviour_state = 0;

    private void Start()
    {
        npc_Dialog.DialogIsFinished += OnDialogFinished;
        //on start play the first dialog
    }

    private void OnDialogFinished()
    {
        //next state?
        behaviour_state++;
    }

    private void Update()
    {
        if((behaviour_state == 0) && (dialog_0_played == false))
        {
            npc_Dialog.SetNewDialogAvailableAndPlay(dialog_0);
            dialog_0_played = true;
        }

        if ((behaviour_state == 1) && (dialog_1_played == false))
        {
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_1);
            dialog_1_played = true;
        }
    }
}
