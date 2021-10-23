using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDoorToCar : MonoBehaviour
{
    [SerializeField] DialogueObject door2FirstLook;
    [SerializeField] GameObject trainMonster;
    [SerializeField] GameObject dialogueBox; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == ("Selected")) { StartCoroutine(FirstInteraction()); }
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
}
