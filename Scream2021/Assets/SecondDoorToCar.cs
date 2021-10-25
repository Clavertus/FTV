using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDoorToCar : MonoBehaviour
{
    [SerializeField] DialogueObject door2FirstLook;
    [SerializeField] GameObject trainMonster;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject physicalDoor;
    [SerializeField] GameObject selectableSideDoor;
    [SerializeField] GameObject sideDoor;
    [SerializeField] Transform doorGapPosition;
    [SerializeField] Transform doorOpenPosition;
    [SerializeField] float openSpeed = 1;
    [SerializeField] Canvas selectableCanvas;
    bool openDoor = false;
    bool firstInteraction = false; 
    AudioSource myAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        myAudioSource = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.OpenDoor);
    }
    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == ("Selected") && !firstInteraction) { StartCoroutine(FirstInteraction()); }
        if (openDoor) {
            Debug.Log("opening");
            physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

            if (physicalDoor.transform.position == doorOpenPosition.position) { gameObject.SetActive(false); }
        }
    }

    IEnumerator FirstInteraction()
    {
        OpenDoorToGap();
        FindObjectOfType<DialogueUI>().ShowDialogue(door2FirstLook); 
        gameObject.tag = ("Untagged");
        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        trainMonster.SetActive(true);
        selectableSideDoor.SetActive(true);
        sideDoor.GetComponent<OpenSideDoor>().PushDoor(); 
        FindObjectOfType<SecondTrain>().TriggerTrain();
        FindObjectOfType<PlayerMovement>().LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
        GetComponent<Selectable>().enabled = false;
        firstInteraction = true;
        selectableCanvas.transform.position = new Vector3(100, 100, 100);

    }
    public void OpenDoor()
    {
        openDoor = true;
        AudioManager.instance.PlayFromGameObject(myAudioSource);
        
    }

    void OpenDoorToGap()
    {
        physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorGapPosition.position, 1000 * Time.deltaTime);
        AudioManager.instance.PlayFromGameObject(myAudioSource);
    }
}
