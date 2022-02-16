using FTV.Saving;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(saved_on_entry == false)
        {
            saved_on_entry = true;
            FindObjectOfType<SavingWrapper>().CheckpointSave();
        }
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
