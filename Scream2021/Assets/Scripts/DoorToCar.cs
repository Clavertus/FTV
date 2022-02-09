using FTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class DoorToCar : MonoBehaviour, ISaveable
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject openDoorDialogue;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject physicalDoor;

    [SerializeField] Transform doorOpenPosition;
    [SerializeField] float openSpeed = 5f; 

    [SerializeField] GameObject windows;

    AudioSource myAudioSource;
    AudioSource myAudioSource2;

    //State
    bool openingDoor = false; 
    int interactionCounter = 0;

    private void OnEnable()
    {
        myAudioSource = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.UnlockDoor);
        myAudioSource2 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.OpenDoor);
    
    }
    void Start()
    {
        windows.SetActive(false);
    }

    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { StartCoroutine(FirstInteraction()); }
        if (gameObject.tag == "Selected" && interactionCounter == 1) { StartCoroutine(SecondInteraction()); }
        if (openingDoor) { OpenDoor(); }
    }

    IEnumerator FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        GetComponent<Selectable>().enabled = false;
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        windows.SetActive(true);
        interactionCounter++;
        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        //gameObject.SetActive(false); 
    }
    public void UnlockDoorSFX() {
        AudioManager.instance.InstantPlayFromGameObject(myAudioSource);  
    }
    IEnumerator SecondInteraction()
    {
        gameObject.tag = ("Untagged");
        GetComponent<Selectable>().enabled = false;
        FindObjectOfType<DialogueUI>().ShowDialogue(openDoorDialogue);
        interactionCounter++;
        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        openingDoor = true;
        AudioManager.instance.InstantPlayFromGameObject(myAudioSource2);  
    }
    void OpenDoor()
    {
        Debug.Log("opening");
        physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

        if(physicalDoor.transform.position == doorOpenPosition.position) { gameObject.SetActive(false); }
    }

    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;
    }
}
