using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MementosInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject secondDialogue;
    [SerializeField] GameObject windows;
    [SerializeField] GameObject gameBoyMemento; 

    //State
    bool checkedWindows = false;
    int interactionCounter = 0;

    void Start()
    {
        windows.SetActive(false);  
    }

    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { FirstInteraction(); }
        if (gameObject.tag == "Selected" && checkedWindows) { SecondInteraction(); }

    }

    void FirstInteraction()
    {    
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        windows.SetActive(true);
        interactionCounter++; 
    }
    public void AreWindowsChecked() { checkedWindows = true; }
    void SecondInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(secondDialogue);
        gameBoyMemento.SetActive(true); 
        interactionCounter++;
    } 
}
