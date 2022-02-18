using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float power;
    public Transform cameraShakeObject;

    Vector3 startPosition;

    void Start()
    {
        //camera = Camera.main.transform;
        if (cameraShakeObject == null) cameraShakeObject = transform;

        startPosition = cameraShakeObject.localPosition;
    }

    void Update()
    {
        cameraShakeObject.localPosition = startPosition + Random.insideUnitSphere * power;
    }
}
