using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformAndShakeObject : MonoBehaviour
{
    [SerializeField] bool deformOnX = false;
    [SerializeField] float deformModifierX = 0.0f;
    [SerializeField] bool deformOnY = false;
    [SerializeField] float deformModifierY = 0.0f;
    [SerializeField] bool deformOnZ = false;
    [SerializeField] float deformModifierZ = 0.0f;
    [SerializeField] bool shake = false;
    [SerializeField] float shakePower = 1.0f;


    float startScaleX = 0.0f;
    float startScaleY = 0.0f;
    float startScaleZ = 0.0f;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startScaleX = transform.localScale.x;
        startScaleY = transform.localScale.y;
        startScaleZ = transform.localScale.z;
        startPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Shake();
        DeformX();
        DeformY();
        DeformZ();
    }

    private void Shake()
    {
        if (shake == false) return;

        this.transform.localPosition = startPosition + UnityEngine.Random.insideUnitSphere * shakePower;
    }

    private void DeformX()
    {
        if (deformOnX == false) return;

        transform.localScale = new Vector3(
            Mathf.Sin(Time.time) * deformModifierX + startScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void DeformY()
    {
        if (deformOnY == false) return;

        transform.localScale = new Vector3(
            transform.localScale.x, Mathf.Sin(Time.time) * deformModifierY + startScaleY, transform.localScale.z);
    }
    private void DeformZ()
    {
        if (deformOnZ == false) return;

        transform.localScale = new Vector3(
            transform.localScale.x, transform.localScale.y, Mathf.Sin(Time.time) * deformModifierZ + startScaleZ);
    }
}

