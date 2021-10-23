using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDoorToCar : MonoBehaviour
{
    [SerializeField] DialogueObject door2FirstLook;
    [SerializeField] GameObject trainMonster;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject physicalDoor;
    [SerializeField] Transform doorOpenPosition;
    [SerializeField] float openSpeed = 1;
    bool openDoor = false;
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
        if(gameObject.tag == ("Selected")) { StartCoroutine(FirstInteraction()); }
        if (openDoor) {
            Debug.Log("opening");
            physicalDoor.transform.position = Vector3.MoveTowards(physicalDoor.transform.position, doorOpenPosition.position, openSpeed * Time.deltaTime);

            if (physicalDoor.transform.position == doorOpenPosition.position) { gameObject.SetActive(false); }
        }
    }

    IEnumerator FirstInteraction()
    {
        FindObjectOfType<DialogueUI>().ShowDialogue(door2FirstLook); 
        gameObject.tag = ("Untagged");
        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        trainMonster.SetActive(true);
        FindObjectOfType<PlayerMovement>().LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
    }
    public void OpenDoor()
    {
        openDoor = true;
        AudioManager.instance.PlayFromGameObject(myAudioSource);
        
    }
}
