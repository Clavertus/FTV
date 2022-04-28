using FTV.Saving;
using FTV.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCinameticFromCollider : MonoBehaviour, ISaveable
{
    [SerializeField] PlayableDirector cinematic = null;
    [SerializeField] NPCDialogue dialogToPlayAfterTriggerEnds = null;

    bool triggerOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        cinematic.played += LockPlayerControl;
        cinematic.stopped += UnlockPlayerControl;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (triggerOnce == false)
            {
                triggerOnce = true;
                cinematic.Play();
                GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private void LockPlayerControl(PlayableDirector pd)
    {
        Debug.Log("LockMenuControl");
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
    }

    private void UnlockPlayerControl(PlayableDirector pd)
    {
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
        FindObjectOfType<MouseLook>().UnlockFromPoint();
        FindObjectOfType<PlayerMovement>().UnlockPlayer();
        FindObjectOfType<DialogueUI>().ShowDialogue(dialogToPlayAfterTriggerEnds);
        FindObjectOfType<SavingWrapper>().CheckpointSave();
    }

    [System.Serializable]
    struct SaveData
    {
        public bool triggerOnce;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.triggerOnce = triggerOnce;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        triggerOnce = data.triggerOnce;
    }
}
