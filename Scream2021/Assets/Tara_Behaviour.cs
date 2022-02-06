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


    [SerializeField] Texture open_texture = null;
    [SerializeField] Texture close_texture = null;
    [SerializeField] Texture speak_texture = null;

    int behaviour_state = 0;
    SkinnedMeshRenderer[] m_Renderers = null;
    private bool eyes_open = true;
    private float openTimer = 0;
    [SerializeField] float openTime = 2f;
    private float closeTimer = 0;
    [SerializeField] float closeTime = 0.25f;
    private float speakTimer = 0;
    [SerializeField] float speakTime = 0.25f;
    private bool speaking = false;

    private void Start()
    {
        m_Renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        npc_Dialog.DialogIsFinished += OnDialogFinished;
        npc_Dialog.DialogNodeIsStarted += OnDialogNodeStarted;
        npc_Dialog.DialogNodeIsEnded += OnDialogNodeFinished;
        //on start play the first dialog
    }
    private void OnDialogNodeStarted()
    {
        speaking = true;
    }
    private void OnDialogNodeFinished()
    {
        speaking = false;
    }

    private void OnDialogFinished()
    {
        Debug.Log("unset");
        speaking = false;
        //next state?
        behaviour_state++;
    }

    private void Update()
    {
        if((behaviour_state == 0) && (dialog_0_played == false))
        {
            Debug.Log("set true 0");
            npc_Dialog.SetNewDialogAvailableAndPlay(dialog_0);
            dialog_0_played = true;
        }

        if ((behaviour_state == 1) && (dialog_1_played == false))
        {
            Debug.Log("set true 1");
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_1);
            dialog_1_played = true;
        }

        TextureSwap();
    }

    private void TextureSwap()
    {
        if(speaking)
        {
            Debug.Log("SpeakBlink");
            SpeakAnimate();
        }
        else
        {
            Debug.Log("EyeBlink");
            EyeBlinkAnimate();
        }
    }

    private void SpeakAnimate()
    {
        if (eyes_open)
        {
            if (speakTimer > speakTime)
            {
                speakTimer = 0;
                eyes_open = false;


                foreach (SkinnedMeshRenderer render in m_Renderers)
                {
                    render.material.mainTexture = speak_texture;
                }
            }
            else
            {
                speakTimer += Time.deltaTime;
            }
        }
        else
        {
            if (speakTimer > speakTime)
            {
                speakTimer = 0;
                eyes_open = true;


                foreach (SkinnedMeshRenderer render in m_Renderers)
                {
                    render.material.mainTexture = open_texture;
                }
            }
            else
            {
                speakTimer += Time.deltaTime;
            }
        }
    }

    private void EyeBlinkAnimate()
    {
        if (eyes_open)
        {
            if (openTimer > openTime)
            {
                openTimer = 0;
                eyes_open = false;


                foreach (SkinnedMeshRenderer render in m_Renderers)
                {
                    render.material.mainTexture = close_texture;
                }
            }
            else
            {
                openTimer += Time.deltaTime;
            }
        }
        else
        {
            if (closeTimer > closeTime)
            {
                closeTimer = 0;
                eyes_open = true;


                foreach (SkinnedMeshRenderer render in m_Renderers)
                {
                    render.material.mainTexture = open_texture;
                }
            }
            else
            {
                closeTimer += Time.deltaTime;
            }
        }
    }
}
