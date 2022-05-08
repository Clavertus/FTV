using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoop : MonoBehaviour
{
    TaraMementoManager taraMementoManager = null;
    // Start is called before the first frame update
    void Start()
    {
        taraMementoManager = FindObjectOfType<TaraMementoManager>();
    }

    int interactionCounter = 0;
    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0)
        {
            Interact();
        }
    }

    public void SetInteractionCounterTo(int value)
    {
        interactionCounter = value;
    }

    private void Interact()
    {
        interactionCounter++;
        Debug.Log("Interact");
        StartCoroutine(taraMementoManager.DoorTriggered());
    }
}
