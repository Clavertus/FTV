using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float power = 0.1f;
    public float shakeTime = 0.2f;
    [Range(0, 50)]
    public int pausePossibility = 50;

    bool triggered = false;
    float currentTriggerTime = 0f;
    Vector3 startPosition;
    bool calledFirstTime = false;

    // Update is called once per frame
    void Update()
    {
        if(triggered)
        {
            if(Random.Range(0, 100) >= pausePossibility)
            {
                transform.position = startPosition + Random.insideUnitSphere * power;
            }

            currentTriggerTime += Time.deltaTime;
            if (currentTriggerTime >= shakeTime)
            {
                triggered = false;
                currentTriggerTime = 0f;
            }
        }
    }

    public void TriggerShake()
    {
        if(!calledFirstTime)
        {
            calledFirstTime = true;
            startPosition = transform.position;
        }
        triggered = true;
        currentTriggerTime = 0f;
    }
}
