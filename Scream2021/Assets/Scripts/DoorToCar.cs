using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToCar : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    
    [SerializeField] GameObject windows;
    

    //State
    
    int interactionCounter = 0;

    void Start()
    {
        windows.SetActive(false);
    }

    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { FirstInteraction(); }
        

    }

    void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        windows.SetActive(true);
        interactionCounter++;
    }
    
    
}
