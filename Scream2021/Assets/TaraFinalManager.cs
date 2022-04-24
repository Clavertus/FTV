using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraFinalManager : MonoBehaviour, ISaveable
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogOnStart = null;
    private PlayerMovement player = null;
    DialogueUI dialogUI = null;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.SetRunEnable(true);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTalkingBackground);
        dialogUI = FindObjectOfType<DialogueUI>();

        StartCoroutine(PlayDialogOnStart());
    }


    private IEnumerator PlayDialogOnStart()
    {
        yield return new WaitForSeconds(0.25f);
        if (dialog_played == false)
        {
            dialogUI.ShowDialogue(dialogOnStart);
            yield return new WaitUntil(() => !dialogUI.dialogueBox.activeSelf);
            dialog_played = true;
            FindObjectOfType<SavingWrapper>().CheckpointSave();
        }

    }

    bool dialog_played = false;
    [System.Serializable]
    struct SaveData
    {
        public bool saved_on_entry;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        Debug.Log("Capture on " + Time.time);
        data.saved_on_entry = dialog_played;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        Debug.Log("Restore on " + Time.time);
        dialog_played = data.saved_on_entry;
    }
}
