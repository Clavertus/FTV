using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    [SerializeField] GameObject doorItself;
    [SerializeField] Transform doorOpenPosition;
    private Vector3 doorClosedPosition;
    [SerializeField] float openSpeed = 5f;

    bool openingDoor = false;
    AudioSource openDoorSound;
    int interactionCounter = 0;

    public Action OnDoorOpened { get; set; }

    public void DoorIsOpened()
    {
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
        if (gameObject.tag == "Selected" && interactionCounter == 0) { FirstInteraction(); }
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
            this.enabled = false; }
    }

}
