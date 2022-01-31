using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainLevelCntrl : MonoBehaviour
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogue0 = null;
    [SerializeField] FTV.Dialog.NPCDialogue dialogue1 = null;
    private bool dialogue0_played = false;
    private bool dialogue1_played = false;

    [SerializeField] float callDialog0After = 2f;
    [SerializeField] float callDialog1After = 20f;
    [SerializeField] float fadeOutDelay = 5f;
    private float timeCounter = 0f;

    private DialogueUI dialogUI = null;
    private MouseLook mouseLook = null;
    private PlayerMovement player = null;

    private bool triggerPlayerDisableControl = false;
    void Awake()
    {
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
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

    private void Update()
    {
        if(!triggerPlayerDisableControl)
        {
            triggerPlayerDisableControl = true;
            mouseLook.LockCamera();
            player.LockPlayer();
        }

        if(dialogue0_played == false)
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
            }
        }

        timeCounter += Time.deltaTime;

        if (dialogUI.dialogueBox.activeSelf)
        {
            timeCounter = 0f;
        }
    }
}
