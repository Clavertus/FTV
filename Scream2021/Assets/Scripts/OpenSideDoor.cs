using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSideDoor : MonoBehaviour
{
    [SerializeField] float pushDistance;
    [SerializeField] float pushCount = 0;
    [SerializeField] float pushDelay = 1;
    [SerializeField] GameObject escapeSelectable; 
    [SerializeField] GameObject doorSelectable;

    public AudioSource myAudioSource; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        myAudioSource = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.PushDoor);

    }
    // Update is called once per frame
    void Update()
    {
        if(doorSelectable.tag == ("Selected") && pushCount <= 2) { PushDoor(); } 
        if(pushCount >= 2) {; doorSelectable.SetActive(false); escapeSelectable.SetActive(true);  } 
    }
    public void PushDoor()
    {
        AudioManager.instance.PlayFromGameObject(myAudioSource);
        Debug.Log("push");
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - pushDistance);
        doorSelectable.tag = ("Untagged");
        
        if (pushCount > 0)
        {
            StartCoroutine(makeSelectableAgain());
        }
        pushCount += pushDistance;
    }
    private IEnumerator makeSelectableAgain()
    {
        yield return new WaitForSeconds(pushDelay);
        doorSelectable.tag = ("Selectable"); 
    }
}
