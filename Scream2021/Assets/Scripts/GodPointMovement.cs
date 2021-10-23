using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPointMovement : MonoBehaviour
{
    [SerializeField] Transform pointA = null;
    [SerializeField] Transform pointB = null;

    [SerializeField] float speed = 10f;
    [SerializeField] float speedBoost = 10f;
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
            transform.rotation = pointA.rotation;
            transform.position = pointA.position;
        }


        if (pointB != null)
        {
            transform.rotation = pointB.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((pointB != null) && enableMovement)
        {
            MoveToPointB();
        }
        
    }

    private void MoveToPointB()
    {
        transform.position = Vector3.MoveTowards(pointA.position, pointB.position, speed * Time.deltaTime);
    }

    

    public void increaseSpeed()
    {
        speed = speedBoost;
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
