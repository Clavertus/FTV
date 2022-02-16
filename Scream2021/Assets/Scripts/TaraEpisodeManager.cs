using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraEpisodeManager : MonoBehaviour, ISaveable
{
    private PlayerMovement player = null;

    bool triggerSave = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        player = FindObjectOfType<PlayerMovement>();
        player.SetRunEnable(true);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTalkingBackground);

        StartCoroutine(DelayedCheckAndSave());
    }

    private IEnumerator DelayedCheckAndSave()
    {
        yield return new WaitForSeconds(0.25f);
        if (saved_on_entry == false)
        {
            saved_on_entry = true;
            Debug.Log("FindObjectOfType<SavingWrapper>().CheckpointSave();");
            FindObjectOfType<SavingWrapper>().CheckpointSave();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("OnLevelWasLoaded");
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
