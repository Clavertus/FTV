using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementosInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject secondDialogue;
    [SerializeField] GameObject windows;

    //State
    bool checkedWindows = false; 
    bool interactedOnce = false;
    bool interactedTwice = false;

    void Start()
    {
        windows.SetActive(false);  
    }

    void Update()
    {
        if (gameObject.tag == "Selected" && !interactedOnce) { FirstInteraction(); }
        if (gameObject.tag == "Selected" && checkedWindows) { SecondInteraction(); }

    }

    void FirstInteraction()
    {    
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        windows.SetActive(true);
        interactedOnce = true; 
    }
    public void AreWindowsChecked() { checkedWindows = true; }
    void SecondInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(secondDialogue);  
        interactedTwice = true; 
    }
}
