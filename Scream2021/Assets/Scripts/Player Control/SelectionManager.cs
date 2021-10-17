using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectionManager : MonoBehaviour
{

    Transform _selection;
    bool selectionLocked;
    void Start()
    {
        selectionLocked = false; 
    }

    public void LockSelection() { selectionLocked = true; }
    public void UnlockSelection() { selectionLocked = false; }
    void Update()
    {
        LookAndSelect();

    }

    private void LookAndSelect()
    {
        if (selectionLocked) { return; }
        //if the player is not looking at something selected, _selection will be null
        //in which case we disable the canvas on the selectable.
        if (_selection != null)
        {
            _selection.GetComponent<Selectable>().DisableSelectable();
            _selection = null;
        }

        //a ray originating at the center of the screen
        var ray = Camera.main.ScreenPointToRay
            (new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        RaycastHit hit;
        //returns true if the raycast hit something, ie the player is looking at an object
        if (Physics.Raycast(ray, out hit))
        {

            var selection = hit.transform;
            if (selection.CompareTag("Selectable"))
            {
                //enable the canvas on the selectable
                selection.GetComponent<Selectable>().DisplaySelectable();
                _selection = selection;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    selection.gameObject.tag = ("Selected");
                    
                }
            }

        }
    }

    
}
