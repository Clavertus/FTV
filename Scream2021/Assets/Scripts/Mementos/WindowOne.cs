using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOne : MonoBehaviour
{
    [SerializeField] DialogueObject windowOneInspection;
    [SerializeField] DialogueObject windowTwoInspection;

    static bool inspectedOnce;
    static bool inspectedTwice; 
    void Start()
    {
        inspectedOnce = false;
        inspectedTwice = false;
          
    }

    void Update()
    {
        if (gameObject.tag == ("Selected") && !inspectedOnce) { FirstInteraction(); }
        if (gameObject.tag == ("Selected") && inspectedOnce) { SecondInteraction(); }

        if (inspectedTwice && Input.GetKeyDown(KeyCode.E)) { gameObject.SetActive(false); } 
    }
    public void InspectedTwice() { inspectedTwice = true; }

    private void FirstInteraction()
    {

        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(windowOneInspection);
        inspectedOnce = true;  
    }

    void SecondInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(windowTwoInspection);
        FindObjectOfType<WindowOne>().InspectedTwice();
        FindObjectOfType<MementosInteractions>().AreWindowsChecked();  
    }
}
