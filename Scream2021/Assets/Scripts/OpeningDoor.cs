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
        //Debug.Log("opening");
        doorItself.transform.position = Vector3.MoveTowards(doorItself.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

        if(doorItself.transform.position == doorOpenPosition.position) { gameObject.SetActive(false); }
    }

}
