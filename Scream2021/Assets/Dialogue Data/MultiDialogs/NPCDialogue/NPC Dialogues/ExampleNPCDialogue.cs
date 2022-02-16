using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FTV.Dialog
{
    public class ExampleNPCDialogue : MonoBehaviour
    {
        [SerializeField] NPCDialogue exampleNPCDialogue = null;

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

            if (gameObject.tag == ("Selected") && inspectedOnce) { inspectedOnce = false; }
        }

        private void FirstInteraction()
        {
            gameObject.tag = ("Untagged");
            FindObjectOfType<DialogueUI>().ShowDialogue(exampleNPCDialogue);
            inspectedOnce = true;
        }
    }
}
