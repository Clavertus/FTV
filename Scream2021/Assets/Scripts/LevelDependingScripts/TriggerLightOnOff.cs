using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLightOnOff : MonoBehaviour
{
    [SerializeField] LightOnOff[] lights = null;

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && (triggered == false))
        {
            triggered = true;
            foreach(LightOnOff light in lights)
            {
                light.enabled = true;
            }
        }
    }
}
