using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] GameObject gameBoyMemento;

    bool checkedWindows = false; 
    int interactionCounter = 0; 
    void Start()
    {
        GetComponent<Selectable>().DisableSelectable();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkedWindows )
        {
            GetComponent<Selectable>().enabled = true; 
            if (gameObject.tag == ("Selected") && interactionCounter == 0)
            {
                FirstInteraction();
            }
        } 
    }
    public void AreWindowsChecked() { checkedWindows = true; }
    void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        gameBoyMemento.SetActive(true);
        interactionCounter++;
    }

}
