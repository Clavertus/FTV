using FTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideTrainManager : MonoBehaviour, ISaveable
{
    [SerializeField] bool playDialogOnStart = true;
    [SerializeField] float delayDialogInSec = 1.5f;
    [SerializeField] FTV.Dialog.NPCDialogue dialogOnStart = null;
    // called zero
    void Awake()
    {
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
        StartCoroutine(PlayDialogOnStart());
    }

    void Start()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }
    private IEnumerator PlayDialogOnStart()
    {
        yield return new WaitForSeconds(delayDialogInSec);
        if(playDialogOnStart)
        {
            if (dialog_played == false)
            {
                FindObjectOfType<DialogueUI>().ShowDialogue(dialogOnStart);
                yield return new WaitUntil(() => !FindObjectOfType<DialogueUI>().dialogueBox.activeSelf);
                dialog_played = true;
                //FindObjectOfType<SavingWrapper>().CheckpointSave();
            }
        }
        else
        {
            dialog_played = true;
        }
    }

    public void TriggerSecondDroneSound()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone2);
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
