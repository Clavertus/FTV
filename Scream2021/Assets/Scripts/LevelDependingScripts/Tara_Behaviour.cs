using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Tara_Behaviour : MonoBehaviour, ISaveable
{
    [Header("Cinematics:")]
    [SerializeField] PlayableDirector lookAtElderGodCinematicSequence = null;
    [SerializeField] PlayableDirector elderGodFinalCinematic = null;
    [SerializeField] GodPointMovement elderGodMovement = null;
    public enum tara_states
    {
        tara_scene_begin,
        tara_scene_lookAtElderGod,
        tara_scene_sit,
        tara_scene_idle,
        tara_scene_waitForPlayer,
        tara_scene_transforming,
        tara_scene_waitForDeath
    }
    [Header("GameObjects:")]
    [SerializeField] GameObject stopZone = null;
    [SerializeField] GameObject taraMemento = null;
    [SerializeField] bool reactOnShelf = true;
    [SerializeField] GameObject taraShelf = null;
    [SerializeField] GameObject[] taraShelfDisableObjects = null;
    [SerializeField] GameObject[] taraShelfEnableObjects = null;
    [SerializeField] GameObject[] taraReferenceObjectsToHide = null;
    [SerializeField] GameObject taraMonster = null;
    [Header("ElderGodReferenceGameObjects:")]
    public bool elderGodDissapeared = false;
    [SerializeField] GameObject trainToDissapear = null;
    [SerializeField] GameObject elderGodToDissapear = null;
    [SerializeField] GameObject elderGodToAppear = null;

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
    [SerializeField] FTV.Dialog.NPCDialogue idle_dialog = null;
    bool idle_dialog_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue shelf_dialog = null;
    bool shelf_dialog_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue changing_dialog = null;
    bool changing_dialog_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue good_dialog = null;
    bool good_dialog_played = false;
    [SerializeField] FTV.Dialog.NPCDialogue run_dialog = null;
    bool run_dialog_played = false;


    [Header("References:")]
    [SerializeField] PlayNPCDialog npc_Dialog = null;

    [Header("Location Points:")]
    [SerializeField] Transform point_0 = null;
    [SerializeField] Transform point_1 = null;
    [SerializeField] Transform point_2 = null;
    [SerializeField] Transform point_3 = null;


    [SerializeField] Texture open_texture = null;
    [SerializeField] Texture close_texture = null;
    [SerializeField] Texture open_Awry_texture = null;
    [SerializeField] Texture close_Awry_texture = null;
    [SerializeField] Texture speak_texture = null;

    public tara_states behaviour_state = tara_states.tara_scene_begin;
    SkinnedMeshRenderer[] m_Renderers = null;
    private bool eyes_open = true;
    private float openTimer = 0;
    [SerializeField] float openTime = 2f;
    private float closeTimer = 0;
    [SerializeField] float closeTime = 0.25f;
    private float speakTimer = 0;
    [SerializeField] float speakTime = 0.25f;
    private bool speaking = false;


    [Header("Sounds:")]
    [SerializeField] float volume = 0.35f;
    [SerializeField] float pitch = 2f;
    public AudioSource[] taraSpeech = new AudioSource[5];

    SavingWrapper savingWrapper = null;
    InGameMenuCotrols inGameMenuControls = null;
    MouseLook mouseLook = null;
    PlayerMovement playerMovement = null;
    DialogueUI dialogUI = null;

    private void Start()
    {
        savingWrapper = FindObjectOfType<SavingWrapper>();
        inGameMenuControls = FindObjectOfType<InGameMenuCotrols>();
        mouseLook = FindObjectOfType<MouseLook>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogUI = FindObjectOfType<DialogueUI>();

        if (elderGodFinalCinematic) elderGodFinalCinematic.played += LockPlayerControl;
        if(elderGodFinalCinematic) elderGodFinalCinematic.stopped += UnlockPlayerControl;

        if (lookAtElderGodCinematicSequence) lookAtElderGodCinematicSequence.played += LockPlayerControl;
        if (lookAtElderGodCinematicSequence) lookAtElderGodCinematicSequence.stopped += UnlockPlayerControl;

        if (stopZone) stopZone.SetActive(true);
        if (taraMemento) taraMemento.SetActive(false);
        
        npc_Dialog.DisableInteraction();
        m_Renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        npc_Dialog.PreTriggerEventCall += OnPreTriggerEventCall;
        npc_Dialog.DialogIsStarted += OnDialogStarted;
        npc_Dialog.DialogIsFinished += OnDialogFinished;
        npc_Dialog.DialogNodeIsStarted += OnDialogNodeStarted;
        npc_Dialog.DialogNodeIsEnded += OnDialogNodeFinished;

        dialog_0_played = false;
        dialog_1_played = false;
        dialog_2_played = false;
        dialog_3_played = false;
        dialog_4_played = false;
        idle_dialog_played = false;
        shelf_dialog_played = false;

        taraSpeech[0] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech1);
        taraSpeech[1] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech2);
        taraSpeech[2] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech3);
        taraSpeech[3] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech4);
        taraSpeech[4] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeech5);
        for(int ix = 0; ix < taraSpeech.Length; ix++)
        {
            taraSpeech[ix].volume = volume;
            taraSpeech[ix].pitch = pitch;
        }

        foreach (GameObject obj in taraShelfDisableObjects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in taraShelfEnableObjects)
        {
            obj.SetActive(false);
        }
    }

    [SerializeField] GameObject transformParticles = null;
    bool triggerAlternateEnd = false;
    private void Update()
    {
        if(countTimeToDeath == true)
        {
            if(triggerAlternateEnd == false)
            {
                TimeCounter += Time.deltaTime;
                if (TimeCounter > timeUntilElderGodBite)
                {
                    triggerAlternateEnd = true;
                    //trigger alternate cinematic
                    if (elderGodFinalCinematic) elderGodFinalCinematic.Play();
                }

            }
        }

        if((behaviour_state == tara_states.tara_scene_begin) && (dialog_0_played == false))
        {
            Debug.LogWarning(behaviour_state);
            npc_Dialog.SetNewDialogAvailableAndPlay(dialog_0);
            dialog_0_played = true;
        }

        else if ((behaviour_state == tara_states.tara_scene_lookAtElderGod) && (dialog_1_played == false))
        {
            Debug.LogWarning(behaviour_state);
            npc_Dialog.SetNewDialogAvailableNoPlayAddPreTrigger(dialog_1);
            dialog_1_played = true;
            GetComponent<NPCMoving>().SetDestination(point_1, false);
        }

        else if ((behaviour_state == tara_states.tara_scene_sit) && (dialog_2_played == false))
        {
            Debug.LogWarning(behaviour_state);
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_2);
            dialog_2_played = true;
            GetComponent<NPCMoving>().SetDestination(point_2, true);

            if(stopZone) stopZone.SetActive(false);
            if(taraMemento) taraMemento.SetActive(true);
        }

        else if ((behaviour_state == tara_states.tara_scene_idle) && (idle_dialog_played == false))
        {
            Debug.LogWarning(behaviour_state);
            npc_Dialog.SetNewDialogAvailableNoPlay(idle_dialog);
            idle_dialog_played = true;

            if(stopZone) stopZone.SetActive(false);
            if(taraMemento) taraMemento.SetActive(true);
        }

        else if ((behaviour_state == tara_states.tara_scene_waitForPlayer) && (dialog_3_played == false))
        {
            Debug.LogWarning(behaviour_state);
            npc_Dialog.SetNewDialogAvailableNoPlay(dialog_3);
            dialog_3_played = true;
            GetComponent<NPCMoving>().SetDestination(point_3, false);

            if (stopZone) stopZone.SetActive(false);
            if (taraMemento) taraMemento.SetActive(true);
        }

        else if ((behaviour_state > tara_states.tara_scene_waitForPlayer) && (dialog_4_played == false))
        {
            Debug.LogWarning(behaviour_state);
            transformParticles.SetActive(true);
            //npc_Dialog.SetNewDialogAvailableNoPlay(dialog_3);
            dialog_4_played = true;
            //GetComponent<NPCMoving>().SetDestination(point_3, false);

            if (taraShelf)
            {
                taraShelf.SetActive(true);
                foreach (GameObject obj in taraShelfDisableObjects)
                {
                    obj.SetActive(false);
                }
                foreach (GameObject obj in taraShelfEnableObjects)
                {
                    obj.SetActive(true);
                }
            }

            if (stopZone) stopZone.SetActive(false);
            if (taraMemento) taraMemento.SetActive(true);
        }


        if (reactOnShelf)
        {
            if ((taraShelf.GetComponentInChildren<PlayDialogOnInspection>().interactionCounter > 0) && (shelf_dialog_played == false))
            {
                if (!dialogUI.dialogueBox.activeSelf)
                {
                    shelf_dialog_played = true;
                    reactOnShelf = false;
                    npc_Dialog.SetNewDialogAvailableAndPlay(shelf_dialog);
                }
            }
        }

        if (!dialogUI.dialogueBox.activeSelf)
        {
            if (GetComponent<NPCMoving>().IsMoving())
            {
                GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.walk);
            }
            else
            {
                if (GetComponent<NPCMoving>().IsSitTarget())
                {
                    //Debug.Log("On sit target");
                    if (GetComponent<NPCAnimationController>().GetCurrentState() != NPCAnimationController.NpcAnimationState.sit)
                    {
                        //Debug.Log("sit down");
                        GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.sit_down);
                    }
                }
                else
                {
                    if (GetComponent<NPCAnimationController>().GetCurrentState() == NPCAnimationController.NpcAnimationState.sit)
                    {
                        GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.stand_up);
                    }
                    else
                    {
                        GetComponent<NPCAnimationController>().SetAnimation(NPCAnimationController.NpcAnimationState.idle);
                    }
                }
            }
        }

        TextureSwap();
    }

    public void SetNPCState(tara_states newState)
    {
        behaviour_state = newState;
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

    bool awryState = false;
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
                    if(awryState == false)
                    {
                        render.material.mainTexture = close_texture;
                    }
                    else
                    {
                        render.material.mainTexture = close_Awry_texture;
                    }
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
                    if (awryState == false)
                    {
                        render.material.mainTexture = open_texture;
                    }
                    else
                    {
                        render.material.mainTexture = open_Awry_texture;
                    }
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
        public bool elderGodDissapeared;
        public bool dialog_0_played;
        public bool dialog_1_played;
        public bool dialog_2_played;
        public bool dialog_3_played;
        public bool dialog_4_played;
        public bool shelf_dialog_played;
        public bool reactOnShelf;
        public int behaviour_state;
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.elderGodDissapeared = elderGodDissapeared;
        data.dialog_0_played = dialog_0_played;
        data.dialog_1_played = dialog_1_played;
        data.dialog_2_played = dialog_2_played;
        data.dialog_3_played = dialog_3_played;
        data.dialog_4_played = dialog_4_played;
        data.shelf_dialog_played = shelf_dialog_played;
        data.reactOnShelf = reactOnShelf;
        data.behaviour_state = (int) behaviour_state;

        GetComponent<NavMeshAgent>().enabled = false;
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.rotation.eulerAngles);
        GetComponent<NavMeshAgent>().enabled = true;

        return data;
    }

    public void RestoreState(object state)
    {
        Debug.LogWarning("Restoring tara state");

        SaveData data = (SaveData)state;
        elderGodDissapeared = data.elderGodDissapeared;
        dialog_0_played = data.dialog_0_played;
        dialog_1_played = data.dialog_1_played;
        dialog_2_played = data.dialog_2_played;
        dialog_3_played = data.dialog_3_played;
        dialog_4_played = data.dialog_4_played;
        shelf_dialog_played = data.shelf_dialog_played;
        reactOnShelf = data.reactOnShelf;
        behaviour_state = (tara_states) data.behaviour_state;

        Debug.LogWarning(data.behaviour_state);
        if (data.behaviour_state > (int) tara_states.tara_scene_waitForPlayer)
        {
            foreach (GameObject obj in taraShelfDisableObjects)
            {
                Debug.LogWarning(obj + " SetDisabled");
                obj.SetActive(false);
            }
            foreach (GameObject obj in taraShelfEnableObjects)
            {
                obj.SetActive(true);
            }
        }

        GetComponent<NavMeshAgent>().enabled = false;
        transform.eulerAngles = data.rotation.ToVector();
        transform.position = data.position.ToVector();
        GetComponent<NavMeshAgent>().enabled = true;

        if(elderGodDissapeared)
        {
            elderGodToDissapear.SetActive(false);
            trainToDissapear.SetActive(false);
        }

        if(data.behaviour_state == (int) tara_states.tara_scene_transforming)
        {
            StartCoroutine(TaraStartTransforming(false));
        }

        else if (data.behaviour_state == (int)tara_states.tara_scene_waitForDeath)
        {
            StartCoroutine(TaraStartWaitForDeath(false));
        }

        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.TaraTalkingBackground);
    }
    #endregion

    #region EVENTS
    private void LockPlayerControl(PlayableDirector pd)
    {
        Debug.Log("LockMenuControl");
        inGameMenuControls.LockMenuControl();
        mouseLook.LockCamera();
        playerMovement.LockPlayer();
    }

    private void UnlockPlayerControl(PlayableDirector pd)
    {
        inGameMenuControls.UnlockMenuControl();
        mouseLook.UnlockFromPoint();

        if (behaviour_state == tara_states.tara_scene_lookAtElderGod)
        {
            EndPreTriggerEvent();
        }
    }
    private void OnPreTriggerEventCall()
    {
        //AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.TaraTalkingBackground);
        if (behaviour_state == tara_states.tara_scene_lookAtElderGod)
        {
            lookAtElderGodCinematicSequence.Play();
        }
    }

    private void EndPreTriggerEvent()
    {
        npc_Dialog.PreTriggerEventFinished();
    }

    private void OnDialogStarted()
    {
        //AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.TaraTalkingBackground);
    }

    int dialogId = 0;
    bool tara_is_transforming = false;
    private void OnDialogNodeStarted(int triggerId)
    {
        Debug.Log("Dialog with id: " + triggerId);
        //speaking = true;
        AudioManager.instance.InstantPlayFromGameObject(taraSpeech[dialogId]);
        dialogId += 1;
        if (dialogId >= taraSpeech.Length) dialogId = 0;
        if (triggerId == (int)1)
        {
            GetComponentInChildren<PlayNPCDialog>().awryState = true;
            awryState = true;
            AudioManager.instance.StopFromAudioManager(soundsEnum.TaraTalkingBackground);
            if(!AudioManager.instance.IsPlaying(soundsEnum.AwryBackground)) AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.AwryBackground);
        }
        else if (triggerId == (int)2)
        {
            tara_is_transforming = true;
        }
        else if (triggerId == (int)3)
        {
            tara_is_transforming = false;
        }
    }
    private void OnDialogNodeFinished(int triggerId)
    {
        Debug.Log("Dialog with id: " + triggerId);
    }

    private void OnDialogFinished()
    {
        if (behaviour_state == tara_states.tara_scene_sit)
        {
            behaviour_state = tara_states.tara_scene_idle;
            Debug.Log("FindObjectOfType<SavingWrapper>().CheckpointSave();");
            savingWrapper.CheckpointSave();
        }
        else if (behaviour_state == tara_states.tara_scene_waitForPlayer)
        {
            if (tara_is_transforming)
            {
                behaviour_state = tara_states.tara_scene_transforming;
                StartCoroutine(TaraStartTransforming(true));
            }
            else
            {
                behaviour_state = tara_states.tara_scene_waitForDeath;
                StartCoroutine(TaraStartWaitForDeath(true));
            }
            Debug.Log("FindObjectOfType<SavingWrapper>().CheckpointSave();");
            savingWrapper.CheckpointSave();
        }
        else if (behaviour_state == tara_states.tara_scene_waitForDeath)
        {
            countTimeToDeath = true;
            //reset "RUN!" dialog
            npc_Dialog.SetNewDialogAvailableNoPlay(run_dialog);
        }
        else if (behaviour_state == tara_states.tara_scene_idle)
        {
            //reset "Nothing to ask" dialog
            idle_dialog_played = false;
        }
        else
        {
            behaviour_state++;
            Debug.Log("FindObjectOfType<SavingWrapper>().CheckpointSave();");
            savingWrapper.CheckpointSave();
        }
    }
    #endregion

    public IEnumerator TaraStartTransforming(bool useFader)
    {
        CameraShaker cameraShaker = FindObjectOfType<CameraShaker>();
        GetComponentInChildren<PlayNPCDialog>().awryState = true;
        awryState = true;
        ProgressTracker.instance.taraEnding = ProgressTracker.endingType.Bad;
        if (!useFader)
        {
            foreach (GameObject obj in taraReferenceObjectsToHide)
            {
                obj.SetActive(false);
            }
            taraMonster.SetActive(true);
        }
        elderGodToAppear.GetComponent<ElderGodAnimationTrigger>().TriggerAppear();
        cameraShaker.enabled = true;
        yield return new WaitForSeconds(1.0f);
        LockPlayerControl(null);
        if(useFader) StartCoroutine(LevelLoader.instance.CutIn());
        yield return new WaitForSeconds(1.75f);
        if(useFader)
        {
            foreach (GameObject obj in taraReferenceObjectsToHide)
            {
                obj.SetActive(false);
            }
            taraMonster.SetActive(true);
        }
        yield return new WaitForSeconds(0.25f);
        if (useFader) StartCoroutine(LevelLoader.instance.CutOut());

        yield return new WaitForSeconds(0.5f);
        UnlockPlayerControl(null);
        cameraShaker.enabled = false;
        yield return new WaitForSeconds(0.25f);
        mouseLook.LockAndLookAtPoint(taraMonster.GetComponent<TaraMonsterController>().GetLookAtPoint());
        dialogUI.ShowDialogue(changing_dialog);
    }

    [SerializeField] float timeUntilElderGodBite = 20f;
    float TimeCounter = 0f;
    public bool countTimeToDeath = false;
    private IEnumerator TaraStartWaitForDeath(bool useFader)
    {
        CameraShaker cameraShaker = FindObjectOfType<CameraShaker>();
        GetComponentInChildren<PlayNPCDialog>().awryState = true;
        awryState = true;
        ProgressTracker.instance.taraEnding = ProgressTracker.endingType.Good;
        elderGodToAppear.GetComponent<ElderGodAnimationTrigger>().TriggerAppear();
        AudioManager.instance.StopFromAudioManager(soundsEnum.TaraTalkingBackground);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTransformBackground);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTransformSFX);
        elderGodMovement.speed *= 0.86f;
        cameraShaker.enabled = true;
        yield return new WaitForSeconds(1.0f);
        LockPlayerControl(null);
        if (useFader) StartCoroutine(LevelLoader.instance.CutIn());
        yield return new WaitForSeconds(2f);
        if (useFader) StartCoroutine(LevelLoader.instance.CutOut());

        yield return new WaitForSeconds(0.5f);
        UnlockPlayerControl(null);
        cameraShaker.enabled = false;
        yield return new WaitForSeconds(0.25f);
        mouseLook.LockAndLookAtPoint(GetComponent<NPCLookAtPlayer>().GetLookAtPoint().position);
        npc_Dialog.SetNewDialogAvailableAndPlay(good_dialog);
    }
}
