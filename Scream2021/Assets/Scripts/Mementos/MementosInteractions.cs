using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementosInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] GameObject windows; 

    void Start()
    {
        windows.SetActive(false);  
    }

    void Update()
    {
        if (gameObject.tag == "Selected") { FirstInteraction(); } 
    }

    void FirstInteraction()
    {    
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        windows.SetActive(true);
    } 
}
