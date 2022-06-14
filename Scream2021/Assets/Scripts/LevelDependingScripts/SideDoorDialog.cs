using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoorDialog : MonoBehaviour
{
    [SerializeField] DialogueObject sideDoorInspection;

    bool inspectedOnce = false;

    DialogueUI dialogUI = null;
    // Start is called before the first frame update
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        inspectedOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == ("Selected") && !inspectedOnce) { FirstInteraction(); }
    }

    private void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        dialogUI.ShowDialogue(sideDoorInspection);
        inspectedOnce = true;
    }

}
