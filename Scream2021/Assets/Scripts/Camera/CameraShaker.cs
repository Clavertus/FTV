using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float power;
    public Transform camera;

    Vector3 startPosition;

    void Start()
    {
        camera = Camera.main.transform;
        startPosition = camera.localPosition;
    }

    void Update()
    {
        camera.localPosition = startPosition + Random.insideUnitSphere * power;
    }
}
