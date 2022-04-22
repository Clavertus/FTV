using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMementoRotation : MonoBehaviour
{
    [SerializeField] MementoObjectInspectingLookAtPart inspectScriptRef = null;
    [SerializeField] RotateMementoAfterInspect rotateScriptRef = null;

    bool triggerOnce = false;
    bool triggerDone = false;
    // Update is called once per frame
    void Update()
    {
        if(triggerOnce == false)
        {
            if (inspectScriptRef.smallObjectTrigger == true)
            {
                triggerOnce = true;
                rotateScriptRef.TriggerRotation();
            }
        }

        if (triggerDone == false)
        {
            if (rotateScriptRef.rotationDone)
            {
                triggerDone = true;
                inspectScriptRef.smallObjectTriggerDone = true;
            }
        }
    }
}
