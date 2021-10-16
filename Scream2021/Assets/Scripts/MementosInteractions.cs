using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementosInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] GameObject dialogueBox; 
    DialogueUI dialogueUi; 
    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.tag == "Selected") { FirstInteraction(); } 
    }

    void FirstInteraction()
    {
         
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue); 
    } 
}
