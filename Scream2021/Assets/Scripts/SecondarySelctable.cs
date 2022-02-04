using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondarySelctable : MonoBehaviour
{
    Transform _selection;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Material originalMat;
    [SerializeField] Material highlightMat; 
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        SecondarySelectionManager();
        if (gameObject.CompareTag("Selectable")) {  }
        
    }

    private void SecondarySelectionManager()
    {
        if (_selection != null)
        {
            _selection.GetComponent<Selectable>().DisableSelectable();
            _selection = null;
        }
        if (gameObject.CompareTag("Selectable"))
        {
            //enable the canvas on the selectable
            GetComponent<Selectable>().DisplaySelectable();
            _selection = gameObject.transform;

            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.gameObject.tag = ("Selected");

                gameObject.GetComponent<Selectable>().DelayedDisableSelectable();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && FindObjectOfType<Examine>().examineMode == true && !dialogueBox.activeSelf)   
        {
            Debug.Log("dpad select");
            gameObject.GetComponent<Renderer>().material = highlightMat;
            gameObject.GetComponent<Selectable>().BypassAndMakeSelectable(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(("Player")))
        { 
            Debug.Log("dpad unselect");
            gameObject.GetComponent<Renderer>().material = originalMat; 
            gameObject.GetComponent<Selectable>().ExitSelectionZone(); 
        }
    }
}
