using FTV.Saving;
using FTV.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaraMementoManager : MonoBehaviour, ISaveable
{
    [SerializeField] Transform startPosition = null;
    [SerializeField] PlayerMovement player = null;
    [SerializeField] GameObject triggerZone = null;
    [SerializeField] NPCDialogue dialogToPlayOnStart = null;
    [SerializeField] NPCDialogue dialogToPlayOnTrigger = null;
    [SerializeField] GameObject DoorToClose = null;
    [SerializeField] Transform DoorClosePosition = null;
    private bool dialogOnStart = true;
    private bool triggerZoneTriggered = false;
    private DialogueUI dialogUI = null;

    // Start is called before the first frame update
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        dialogUI.OnDialogShowEnd += OnDialogFinished;

        if (LevelLoader.instance.GetSavedTransform() != null)
        {
            startPosition.eulerAngles = LevelLoader.instance.GetSavedTransform().eulerAngles;
            startPosition.position = LevelLoader.instance.GetSavedTransform().position;
        }
        else
        {
            startPosition.eulerAngles = player.transform.eulerAngles;
            startPosition.position = player.transform.position;
        }

        SetPlayerToSavedTransform();
    }

    // Update is called once per frame
    void Update()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTalkingBackground);
        dialogUI = FindObjectOfType<DialogueUI>();

        if (dialogOnStart == true)
        {
            dialogOnStart = false;
            dialogUI.ShowDialogue(dialogToPlayOnStart);
        }

        if(triggerZone.activeSelf)
        {
            if(triggerZoneTriggered)
            {
                triggerZone.SetActive(false);
            }
        }
    }

    public IEnumerator DoorTriggered()
    {
        player.LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();

        yield return LevelLoader.instance.FadeIn();


        DoorToClose.transform.position = DoorClosePosition.position;
        DoorToClose.GetComponent<OpeningDoor>().ResetToDefaultState();

        FindObjectOfType<TriggerLoop>().SetInteractionCounterTo(0);

        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Change5);

        SetPlayerToSavedTransform();

        yield return LevelLoader.instance.FadeOut();

        player.UnlockPlayer();
        FindObjectOfType<MouseLook>().UnlockCamera();
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
    }

    private void OnDialogFinished(FTV.Dialog.NPCDialogue dialog)
    {
        if (dialog == dialogToPlayOnStart)
        {
            SavingWrapper.instance.CheckpointSave();
        }

        else if (dialog == dialogToPlayOnTrigger)
        {
            triggerZoneTriggered = true;
        }
    }

    private void SetPlayerToSavedTransform()
    {
        player.enabled = false;
        while (player.transform.position != startPosition.position)
        {
            player.transform.eulerAngles = startPosition.eulerAngles;
            player.transform.position = startPosition.position;
        }
        player.enabled = true;
        Debug.Log("SetPlayerToSavedTransform");
    }
    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public bool dialogOnStart;
        public bool triggerZoneTriggered;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.dialogOnStart = dialogOnStart;
        data.triggerZoneTriggered = triggerZoneTriggered;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        dialogOnStart = data.dialogOnStart;
        triggerZoneTriggered = data.triggerZoneTriggered;
    }
    #endregion
}
