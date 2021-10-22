using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [SerializeField] float minLightOn = 1f;
    [SerializeField] float maxLightOn = 1f;
    [SerializeField] float minLightOff = 1f;
    [SerializeField] float maxLightOff = 1f;

    [SerializeField] Light myLight = null;

    float TimeCount = 0f;
    float LightOffTime = 0.1f;
    float LightOnTime = 0.1f;
    private void Start()
    {
        AudioManager.instance.PlayFromAudioManager(soundsEnum.TV);
    }

    // Update is called once per frame
    void Update()
    {
        if(myLight.enabled)
        {
            if (TimeCount >= LightOffTime)
            {
                myLight.enabled = false;
                LightOffTime = Random.Range(minLightOff, maxLightOff);
                TimeCount = 0f;
                return;
            }
        }
        else
        {
            if (TimeCount >= LightOnTime)
            {
                myLight.enabled = true;
                LightOnTime = Random.Range(minLightOn, maxLightOn);
                TimeCount = 0f;
                return;
            }
        }

        TimeCount += Time.deltaTime;
    }
}
