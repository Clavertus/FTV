using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObjectAfterInspect : MonoBehaviour
{
    [SerializeField] bool takeObjectToInventory = false;
    [SerializeField] GameObject hideObject = null;
    [SerializeField] GameObject revealObject = null;

    public int interactionCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        interactionCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (takeObjectToInventory == false) return;

        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        interactionCounter++;
        revealObject.SetActive(true);
        hideObject.SetActive(false);
    }
}
