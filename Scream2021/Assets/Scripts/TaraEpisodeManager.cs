using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraEpisodeManager : MonoBehaviour, ISaveable
{

    [SerializeField] GameObject closeDoorInspect = null;
    [SerializeField] Transform doorToClose = null;
    [SerializeField] Transform doorClosePosition = null;
    [SerializeField] PlayDialogOnInspection doorDialog = null;


    [SerializeField] MementoObjectInspectingLookAtPart mementoInspecting = null;

    private PlayerMovement player = null;
    DialogueUI dialogUI = null;
    // Start is called before the first frame update
    void Start()
    {
        closeDoorInspect.SetActive(false);
        player = FindObjectOfType<PlayerMovement>();
        player.SetRunEnable(true);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTalkingBackground);
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    bool triggerSceneEnd = false;
    private void Update()
    {
        if(doorToClose.position != doorClosePosition.position)
        {
            if (mementoInspecting.smallObjInteractionCounter > 0 && !dialogUI.dialogueBox.activeSelf)
            {
                if (FindObjectOfType<Examine>().GetExamineMode() == false)
                {
                    doorToClose.position = doorClosePosition.position;
                    AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.OpenDoor);
                    closeDoorInspect.SetActive(true);
                }
            }
        }

        if((triggerSceneEnd == false) && (doorDialog.interactionCounter > 0) && (!dialogUI.dialogueBox.activeSelf))
        {
            triggerSceneEnd = true;
            StartSceneTransition();
        }
    }

    private void StartSceneTransition()
    {
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();

        AudioManager.instance.StopAllSounds();
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }

    bool saved_on_entry = false;
    [System.Serializable]
    struct SaveData
    {
        public bool saved_on_entry;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.saved_on_entry = saved_on_entry;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        saved_on_entry = data.saved_on_entry;
    }
}
