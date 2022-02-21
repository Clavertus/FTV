using FTV.Saving;
using FTV.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaraMementoManager : MonoBehaviour, ISaveable
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] NPCDialogue dialogToPlayOnStart = null;
    private bool dialogOnStart = true;
    private DialogueUI dialogUI = null;
    // Start is called before the first frame update
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        dialogUI.OnDialogShowEnd += OnDialogFinished;
        if (LevelLoader.instance.GetSavedTransform() != null)
        {
            player.transform.rotation = LevelLoader.instance.GetSavedTransform().rotation;
            player.transform.position = LevelLoader.instance.GetSavedTransform().position;
        }
    }

    private void OnDialogFinished(FTV.Dialog.NPCDialogue dialog)
    {
        if(dialog == dialogToPlayOnStart)
        {
            SavingWrapper.instance.CheckpointSave();
        }
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
    }

    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public bool dialogOnStart;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.dialogOnStart = dialogOnStart;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        dialogOnStart = data.dialogOnStart;
    }
    #endregion
}
