using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tara_Behaviour : MonoBehaviour, ISaveable
{
    enum tara_states
    {
        tara_scene_begin,
        tara_scene_lookAtElderGod,
        tara_scene_idle
    }

    [Header("Dialogs:")]
    [SerializeField] FTV.Dialog.NPCDialogue dialog_0 = null;
    bool dialog_0_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_1 = null;
    bool dialog_1_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_2 = null;
    bool dialog_2_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_3 = null;
    bool dialog_3_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue dialog_4 = null;
    bool dialog_4_played = false;


    [Header("References:")]
    [SerializeField] PlayNPCDialog npc_Dialog = null;

    [Header("Location Points:")]
    [SerializeField] Transform point_0 = null;
    [SerializeField] Transform point_1 = null;
    [SerializeField] Transform point_2 = null;


    [SerializeField] Texture open_texture = null;
    [SerializeField] Texture close_texture = null;
    [SerializeField] Texture speak_texture = null;

    tara_states behaviour_state = tara_states.tara_scene_begin;
    SkinnedMeshRenderer[] m_Renderers = null;
    private bool eyes_open = true;
    private float openTimer = 0;
    [SerializeField] float openTime = 2f;
    private float closeTimer = 0;
    [SerializeField] float closeTime = 0.25f;
    private float speakTimer = 0;
    [SerializeField] float speakTime = 0.25f;
    private bool speaking = false;


    public AudioSource[] taraSpeech = new AudioSource[5];
    private void Start()
    {
        npc_Dialog.DisableInteraction();
        m_Renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        npc_Dialog.DialogIsStarted += OnDialogStarted;
        npc_Dialog.DialogIsFinished += OnDialogFinished;
        npc_Dialog.DialogNodeIsStarted += OnDialogNodeStarted;
        npc_Dialog.DialogNodeIsEnded += OnDialogNodeFinished;
        //on start play the first dialog

        taraSpeech[0] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech1);
        taraSpeech[1] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech2);
        taraSpeech[2] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech3);
        taraSpeech[3] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech4);
        taraSpeech[4] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech5);
    }

    private void OnDialogStarted()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.TaraTalkingBackground);
    }

    int dialogId = 0;
    private void OnDialogNodeStarted()
    {
        //speaking = true;
        AudioManager.instance.InstantPlayFromGameObject(taraSpeech[dialogId]);
        dialogId += 1;
        if (dialogId >= taraSpeech.Length) dialogId = 0;
    }
    private void OnDialogNodeFinished()
    {
        //speaking = false;
    }

    private void OnDialogFinished()
    {
        if(behaviour_state != tara_states.tara_scene_idle)
        {
            behaviour_state++;
        }
        else
        {
            //reset "Nothing to ask" dialog
            dialog_2_played = false;
        }

        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.TaraTalkingBackground);
        //find place to save game progress
        FindObjectOfType<SavingWrapper>().CheckpointSave();
    }

    private void Update()
    {
        if((behaviour_state == tara_states.tara_scene_begin) && (dialog_0_played == false))
        {
            npc_Dialog.SetNewDialogAvailableAndPlay(dialog_0);
            dialog_0_played = true;
        }

        else if ((behaviour_state == tara_states.tara_scene_lookAtElderGod) && (dialog_1_played == false))
        {
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_1);
            dialog_1_played = true;
            GetComponent<NPCMoving>().SetDestination(point_1);
        }

        else if ((behaviour_state == tara_states.tara_scene_idle) && (dialog_2_played == false))
        {
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_2);
            dialog_2_played = true;
        }

        if (!FindObjectOfType<DialogueUI>().dialogueBox.activeSelf)
        {
            if (GetComponent<NPCMoving>().IsMoving())
            {
                GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.walk);
            }
            else
            {
                GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.idle);
            }
        }

        TextureSwap();
    }

    #region REGION_TEXTURING
    private void TextureSwap()
    {
        if(speaking)
        {
            SpeakAnimate();
        }
        else
        {
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
    #endregion

    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public bool dialog_0_played;
        public bool dialog_1_played;
        public bool dialog_2_played;
        public bool dialog_3_played;
        public bool dialog_4_played;
        public int behaviour_state;
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.dialog_0_played = dialog_0_played;
        data.dialog_1_played = dialog_1_played;
        data.dialog_2_played = dialog_2_played;
        data.dialog_3_played = dialog_3_played;
        data.dialog_4_played = dialog_4_played;
        data.behaviour_state = (int) behaviour_state;

        GetComponent<NavMeshAgent>().enabled = false;
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.rotation.eulerAngles);
        GetComponent<NavMeshAgent>().enabled = true;

        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        dialog_0_played = data.dialog_0_played;
        dialog_1_played = data.dialog_1_played;
        dialog_2_played = data.dialog_2_played;
        dialog_3_played = data.dialog_3_played;
        dialog_4_played = data.dialog_4_played;
        behaviour_state = (tara_states) data.behaviour_state;

        GetComponent<NavMeshAgent>().enabled = false;
        transform.eulerAngles = data.rotation.ToVector();
        transform.position = data.position.ToVector();
        GetComponent<NavMeshAgent>().enabled = true;

        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.TaraTalkingBackground);
    }
    #endregion
}
