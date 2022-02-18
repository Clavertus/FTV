using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoorDialog : MonoBehaviour
{
    [SerializeField] DialogueObject sideDoorInspection;

    bool inspectedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
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
        FindObjectOfType<DialogueUI>().ShowDialogue(sideDoorInspection);
        inspectedOnce = true;
    }

}
