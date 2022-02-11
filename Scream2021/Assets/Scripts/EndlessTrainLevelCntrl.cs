using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainLevelCntrl : MonoBehaviour, ISaveable
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogue0 = null;
    [SerializeField] FTV.Dialog.NPCDialogue dialogue1 = null;
    private bool fadeOut_played = false;
    private bool dialogue0_played = false;
    private bool dialogue1_played = false;

    [SerializeField] float callDialog0After = 2f;
    [SerializeField] float callDialog1After = 20f;
    [SerializeField] float fadeOutDelay = 5f;
    private float timeCounter = 0f;
    private float fadeOutCounter = 0f;

    private DialogueUI dialogUI = null;
    private MouseLook mouseLook = null;
    private PlayerMovement player = null;

    private bool triggerPlayerDisableControl = false;
    TrainEffectController[] trains = null;

    void Awake()
    {
        trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
    }

    private void Start()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TrainAmbientLoop);
        dialogUI = FindObjectOfType<DialogueUI>();
        mouseLook = FindObjectOfType<MouseLook>();
        player = FindObjectOfType<PlayerMovement>();
    }

    bool dialog_enabled = true;

    internal void FlickOff()
    {
        flickOn = false;
        foreach (TrainEffectController train in trains)
        {
            StartCoroutine(train.StopLightFlick());
        }
    }

    private void Update()
    {
        if (dialog_enabled)
        {
            TriggerDialogs();
        }

        if(flickOn)
        {
            if(flickTime < flickCounter)
            {
                flickCounter = 0.0f;
                foreach (TrainEffectController train in trains)
                {
                    float time = UnityEngine.Random.Range(0.01f, flickTimeMax);
                    train.FlickerLightForTime(time);
                    flickTime = time;
                }
            }
            flickCounter += Time.deltaTime;
        }
    }

    float flickCounter = 0.0f;
    float flickTime = 0.0f;
    float flickTimeMax = 0.0f;
    bool flickOn = false;
    public void TriggerFlick(float duration)
    {
        flickOn = true;
        flickTimeMax = duration;
        foreach (TrainEffectController train in trains)
        {
            train.FlickerLightForTime(flickTimeMax);
        }
    }

    public IEnumerator TriggerLight(float after, float duration)
    {
        yield return new WaitForSeconds(after);
        foreach (TrainEffectController train in trains)
        {
            StartCoroutine(train.SetLightOff());
        }
        yield return new WaitForSeconds(duration);
        foreach (TrainEffectController train in trains)
        {
            StartCoroutine(train.SetLightOn());
        }
    }

    private void TriggerDialogs()
    {
        if (!triggerPlayerDisableControl)
        {
            triggerPlayerDisableControl = true;
            Debug.Log("LockMenuControl");
            FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
            mouseLook.LockCamera();
            player.LockPlayer();
        }

        if (dialogue0_played == false)
        {
            if (timeCounter > callDialog0After + fadeOutDelay)
            {
                dialogue0_played = true;
                //play the first dialog
                dialogUI.ShowDialogue(dialogue0);
            }
        }
        else
        {
            if ((timeCounter > callDialog1After) && (!dialogue1_played) && !dialogUI.dialogueBox.activeSelf)
            {
                dialogue1_played = true;
                //play the first dialog
                dialogUI.ShowDialogue(dialogue1);
                dialog_enabled = false;
            }
        }

        timeCounter += Time.deltaTime;

        if (dialogUI.dialogueBox.activeSelf)
        {
            timeCounter = 0f;
        }

        if (fadeOutCounter >= fadeOutDelay)
        {
            if (fadeOut_played == false)
            {
                FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
                mouseLook.UnlockCamera();
                player.UnlockPlayer();
                fadeOut_played = true;
            }
        }
        else
        {
            fadeOutCounter += Time.deltaTime;
        }
    }


    [System.Serializable]
    struct SaveData
    {
        public bool dialog_enabled;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.dialog_enabled = dialog_enabled;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        dialog_enabled = data.dialog_enabled;
    }
}
