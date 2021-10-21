using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteractions : MonoBehaviour
{
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] GameObject gameBoyMemento;
    [SerializeField] GameObject dPadClone;

    [SerializeField] float applyMementosOffset = .5f;
    [SerializeField] float moveToSymbolRate = .1f;


    
    GameObject dClone;

    bool pocketedDPad = false;
    bool checkedWindows = false;
    bool dPadInstantiated = false; 

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

        if (pocketedDPad && interactionCounter == 1 && gameObject.tag == "Selected") 
        {
            ApplyDPad();
        }

        if (dPadInstantiated) { dClone.transform.Translate(new Vector3(0, 0, -moveToSymbolRate) * Time.deltaTime); }
    }
    public void AreWindowsChecked() { checkedWindows = true; }
    public void IsDPadPocketed() { pocketedDPad = true; } 
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
        var transformOffset = new Vector3(transform.position.x, transform.position.y + .7f, transform.position.z + applyMementosOffset);  
        dClone = Instantiate(dPadClone, transformOffset, Quaternion.identity);
        dPadInstantiated = true;  
        interactionCounter++;
    }
}
