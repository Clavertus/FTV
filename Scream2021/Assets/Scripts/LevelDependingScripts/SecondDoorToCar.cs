using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    bool shakeDoor = false;
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

    [SerializeField] float shakeDoorTime = .25f;
    [SerializeField] float shakeDoorPower = 1.5f;
    Vector3 savedPosition = Vector3.zero;
    float shakeTimeCnt = 0f;
    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == ("Selected") && !firstInteraction) { StartCoroutine(FirstInteraction()); }
        if (openDoor)
        {
            shakeDoor = false;
            Debug.Log("opening");
            physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

            if (physicalDoor.transform.position == doorOpenPosition.position) { gameObject.SetActive(false); }
        }
        else if(shakeDoor)
        {
            if(shakeDoorTime > shakeTimeCnt)
            {
                physicalDoor.transform.position = new Vector3(physicalDoor.transform.position.x + Random.Range(-shakeDoorPower, shakeDoorPower) * Time.deltaTime, physicalDoor.transform.position.y, physicalDoor.transform.position.z);
                shakeTimeCnt += Time.deltaTime;
            }
            else
            {
                shakeDoor = false;
                shakeTimeCnt = 0f;
                physicalDoor.transform.position = savedPosition;
            }
        }
    }

    [SerializeField] PlayableDirector lookAtDoorCinematic = null;
    IEnumerator FirstInteraction()
    {
        OpenDoorToGap();
        lookAtDoorCinematic.Play();
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
        AudioManager.instance.InstantPlayFromGameObject(myAudioSource);
    }

    void OpenDoorToGap()
    {
        physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorGapPosition.position, 1000 * Time.deltaTime);
        AudioManager.instance.InstantPlayFromGameObject(myAudioSource);
        savedPosition = physicalDoor.transform.position;
    }

    public void ShakeDoor()
    {
        if(savedPosition == Vector3.zero)
        {
            savedPosition = physicalDoor.transform.position;
        }
        shakeDoor = true;
    }
}
