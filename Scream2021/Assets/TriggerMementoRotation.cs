using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMementoRotation : MonoBehaviour
{
    [SerializeField] MementoObjectInspectingLookAtPart inspectScriptRef = null;
    [SerializeField] RotateMementoAfterInspect rotateScriptRef = null;
    [SerializeField] bool triggerEffect = false;
    [SerializeField] GameObject TriggerEffectCanvas = null;
    [SerializeField] float EffectTimeInSec = 0f;

    [SerializeField] bool triggerSound = false;
    [SerializeField] soundsEnum triggerSoundEnum = soundsEnum.IntroDrone;

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
                if(triggerEffect)
                {
                    StartCoroutine(TriggerEffect());
                }
            }
        }
    }
    private IEnumerator TriggerEffect()
    {
        TriggerEffectCanvas.SetActive(true);
        if(triggerSound)
        {
            AudioManager.instance.PlayOneShotFromAudioManager(triggerSoundEnum);
        }
        yield return new WaitForSeconds(EffectTimeInSec);
        TriggerEffectCanvas.SetActive(false);
    }
}
