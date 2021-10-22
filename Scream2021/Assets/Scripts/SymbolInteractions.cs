using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject zipperDialogue;
    [SerializeField] DialogueObject frameStandDialogue;
    [SerializeField] GameObject dialogueBox; 

    [SerializeField] GameObject gameBoyMemento;
    [SerializeField] GameObject zipperMemento;
    [SerializeField] GameObject photoMemento;

    [SerializeField] GameObject dPadClone;
    [SerializeField] GameObject zipperClone;
    [SerializeField] GameObject elderGodMove;

    [SerializeField] GameObject chain;
    [SerializeField] GameObject chainDPad;
    [SerializeField] GameObject chainZip;

    [SerializeField] float applyMementosOffset = .5f;
    [SerializeField] float moveToSymbolRate = .1f;

    [SerializeField] Material dPadMat;
    [SerializeField] Material zipperMat;
    [SerializeField] Material frameMat;

    [SerializeField] GameObject trainMonster; 


    bool pocketed = false;
    bool checkedWindows = false;

    string pocketItem;
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
            chain.SetActive(true); 
            GetComponent<Selectable>().enabled = true; 
            if (gameObject.tag == ("Selected") && interactionCounter == 0)
            {
                FirstInteraction();
            }
        } 

        if (pocketed && interactionCounter == 1 && gameObject.tag == "Selected" && pocketItem == ("DPad")) 
        {
            ApplyDPad();
        }

        if (pocketed && interactionCounter == 2 && gameObject.tag == "Selected" && pocketItem == ("Zipper"))
        {
            ApplyZipper();
        }
        if (pocketed && interactionCounter == 3 && gameObject.tag == "Selected" && pocketItem == ("Frame Stand"))
        {
            StartCoroutine(ApplyFrameStand());
        }

    }
    public void AreWindowsChecked() { checkedWindows = true; }
    public void IsPocketed(string pocketedItem) { pocketed = true; pocketItem = pocketedItem; } 
    void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        gameBoyMemento.SetActive(true);
        interactionCounter++;
    }

    void ApplyDPad()
    {
        gameObject.tag = ("Untagged");
        chainDPad.GetComponent<MeshRenderer>().material = dPadMat;
        interactionCounter++;
        zipperMemento.SetActive(true);
        gameBoyMemento.SetActive(false);
    }

    void ApplyZipper()
    {
        gameObject.tag = ("Untagged");
        chainZip.GetComponent<MeshRenderer>().material = zipperMat; 
        elderGodMove.SetActive(true);
        FindObjectOfType<DialogueUI>().ShowDialogue(zipperDialogue);
        photoMemento.SetActive(true);
        zipperMemento.SetActive(false); 
        interactionCounter++;
    }

    IEnumerator ApplyFrameStand() 
    {
        gameObject.tag = ("Untagged");
        chain.GetComponent<MeshRenderer>().material = frameMat;
        FindObjectOfType<DialogueUI>().ShowDialogue(frameStandDialogue);
        interactionCounter++;

        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        StartMonster();

    }

    void StartMonster()
    {
        trainMonster.SetActive(true);  
    }

}
