using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPointMovement : MonoBehaviour
{
    [SerializeField] Transform pointA = null;
    [SerializeField] Transform pointB = null;
    [SerializeField] Transform pointC = null;

    [SerializeField] public float speed = 10f;
    [SerializeField] float speedBoost = 10f;
    [SerializeField] bool changeRotationToPoints = false;
    bool enableMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        if (pointA == null)
        {
            pointA = transform;
        }
        else
        {
            if(changeRotationToPoints)
            {
                transform.rotation = pointA.rotation;
            }
            transform.position = pointA.position;
        }


        if (pointC != null)
        {
            if (changeRotationToPoints)
            {
                transform.rotation = pointC.rotation;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((pointC != null) && enableMovement)
        {
            MoveToPointC();
        }
    }

    private void MoveToPointC()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointC.position, speed * Time.deltaTime);
    }

    public void SetToPointB()
    {
        transform.position = pointB.position;
    }


    public void increaseSpeed()
    {
        speed = speed + speedBoost;
    }

    public void OnEnable()
    {
        enableMovement = true;
    }

    private void OnDisable()
    {
        enableMovement = false;
    }
}
