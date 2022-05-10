using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour, ISaveable
{
    [SerializeField] GameObject doorItself;
    [SerializeField] Transform doorOpenPosition;
    private Vector3 doorClosedPosition;
    [SerializeField] float openSpeed = 5f;
    [SerializeField] bool saveDoorState = false;
    [SerializeField] bool checkpointSave = false;

    bool openingDoor = false;
    AudioSource openDoorSound;
    int interactionCounter = 0;

    public Action OnDoorOpened { get; set; }

    public void DoorIsOpened()
    {
        if(saveDoorState) door_was_unlocked = true;
        Debug.Log("DoorIsOpened");
        OnDoorOpened?.Invoke();
    }

    private void OnEnable()
    {
        //openDoorSound = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.OpenDoor);
    }
    void Start()
    {
        doorClosedPosition = doorItself.transform.position;
    }

    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) {
            if(checkpointSave)
            {
                FindObjectOfType<SavingWrapper>().CheckpointSave();
            }
            FirstInteraction(); 
        }
        if (openingDoor) { OpenDoor(); }
    }

    private void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        interactionCounter++;
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.OpenDoor);
        openingDoor = true;
    }

    void OpenDoor()
    {
        doorItself.transform.position = Vector3.MoveTowards(doorItself.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

        if(Vector3.Distance(doorItself.transform.position, doorOpenPosition.position) <= 0.05f)
        {
            DoorIsOpened();
            GetComponent<Selectable>().enabled = false;
            this.enabled = false; 
        }
    }

    bool door_was_unlocked = false;
    [System.Serializable]
    struct SaveData
    {
        public bool door_was_unlocked;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.door_was_unlocked = door_was_unlocked;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        door_was_unlocked = data.door_was_unlocked;
        if(door_was_unlocked)
        {
            gameObject.tag = ("Untagged");
            doorItself.transform.position = doorOpenPosition.position;
            GetComponent<Selectable>().enabled = false;
        }
    }

    public void ResetToDefaultState()
    {
        interactionCounter = 0;
        door_was_unlocked = false;
        openingDoor = false;
        GetComponent<Selectable>().enabled = true;
        this.enabled = true;
    }
}
