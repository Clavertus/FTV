using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObjectAfterInspect : MonoBehaviour
{
    [SerializeField] bool takeObjectToInventory = false;
    [SerializeField] GameObject hideObject = null;
    [SerializeField] GameObject revealObject = null;
    [SerializeField] bool delayEnable = false;
    [SerializeField] float delayTime = 0.15f;

    public int interactionCounter = 0;
    float delayCounter = 0.0f;

    private void OnEnable()
    {
        if(delayEnable)
        {
            delayCounter = 0.0f;
            GetComponent<Selectable>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        interactionCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(delayEnable)
        {
            if(delayCounter > delayTime)
            {
                delayEnable = false;
                GetComponent<Selectable>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                delayCounter += Time.deltaTime;
            }
            return;
        }

        if (takeObjectToInventory == false) return;

        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        interactionCounter++;
        if (revealObject) revealObject.SetActive(true);
        if(hideObject) hideObject.SetActive(false);
    }
}
