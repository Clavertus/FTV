using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondarySelectableAndTakeable : MonoBehaviour
{
    Transform _selection;
    //[SerializeField] GameObject dialogueBox;
    [SerializeField] bool changeMaterialOnSelection = true;
    [SerializeField] bool allowMoreThanOneInspection = false;
    [SerializeField] Material originalMat;
    [SerializeField] Material highlightMat;

    GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = FindObjectOfType<DialogueUI>().dialogueBox;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        SecondarySelectionManager();
        if (gameObject.CompareTag("Selectable")) 
        { 
            /* place for some function here */
        }
        
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

                if (allowMoreThanOneInspection == false)
                {
                    gameObject.GetComponent<Selectable>().DelayedDisableSelectable();
                }
            }
        }
        else
        {
            ChangeMaterialTo(originalMat);
        }
    }

    private void ChangeMaterialTo(Material material)
    {
        if (changeMaterialOnSelection)
        {
            if (gameObject.GetComponent<Renderer>())
            {
                gameObject.GetComponent<Renderer>().material = material;
            }
            else if (gameObject.GetComponent<ExamineObjectReferences>())
            {
                gameObject.GetComponent<ExamineObjectReferences>().GetSmallObjRenderer().material = material;
            }
            else
            {
                Debug.LogError("No render find on small object!");
            }
        }
    }

    private void OnDisable()
    {
        ChangeMaterialTo(originalMat);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && FindObjectOfType<Examine>().GetExamineMode() == true && !dialogueBox.activeSelf)   
        {
            ChangeMaterialTo(highlightMat);
            gameObject.GetComponent<Selectable>().BypassAndMakeSelectable(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(("Player")))
        {
            //Debug.Log("dpad unselect");
            ChangeMaterialTo(highlightMat);
            gameObject.GetComponent<Selectable>().ExitSelectionZone(); 
        }
    }
}
