using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] CameraShaker cameraShaker = null;
    // Start is called before the first frame update
    void Start()
    {
        if (!cameraShaker) Debug.LogError("Assign camera shaker to this script!");
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraShaker.enabled == true)
        {
            if (shakeTimer > shakeTime)
            {
                cameraShaker.ResetPower();
                cameraShaker.enabled = false;
            }
            else
            {
                shakeTimer += Time.deltaTime;
            }
        }
    }

    float shakeTime = 0f;
    float shakeTimer = 0f;
    public void ShakeCameraForTime(float time, float Power)
    {
        cameraShaker.power = Power;
        cameraShaker.enabled = true;
        shakeTime = time;
        shakeTimer = 0f;
    }
}
