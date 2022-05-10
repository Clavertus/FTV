using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [SerializeField] float minLight_Off = 1f;
    [SerializeField] float maxLight_Off = 1f;
    [SerializeField] float minLight_On = 1f;
    [SerializeField] float maxLight_On = 1f;

    [SerializeField] Light myLight = null;

    float TimeCount = 0f;
    float LightOffTime = 0.1f;
    float LightOnTime = 0.1f;

    private void Start()
    {
        if(!myLight)
        {
            myLight = GetComponent<Light>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(myLight.enabled)
        {
            if (TimeCount >= LightOffTime)
            {
                myLight.enabled = false;
                LightOffTime = Random.Range(minLight_On, maxLight_On);
                TimeCount = 0f;
                return;
            }
        }
        else
        {
            if (TimeCount >= LightOnTime)
            {
                myLight.enabled = true;
                LightOnTime = Random.Range(minLight_Off, maxLight_Off);
                TimeCount = 0f;
                return;
            }
        }

        TimeCount += Time.deltaTime;
    }

    private void OnDisable()
    {
        myLight.enabled = true;
        TimeCount = 0f;
        LightOffTime = 0.1f;
        LightOnTime = 0.1f;
    }
}
