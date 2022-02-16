using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraEpisodeManager : MonoBehaviour, ISaveable
{
    private PlayerMovement player = null;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.SetRunEnable(true);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TaraTalkingBackground);
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
