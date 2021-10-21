using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTunnel : MonoBehaviour
{
    public float zDestination;
    public float speed;
    public float acceleration;
    public bool isFirstTunnel;
    public float zStart;

    private Vector3 position;
    void Awake()
    {
        zStart = Array.Find(FindObjectsOfType<MovingTunnel>(), item => item.isFirstTunnel).transform.position.z;
    }

    void Update()
    {
        float movingSpeed = Mathf.Lerp(0, speed, acceleration);
        position = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, position.y, zDestination), movingSpeed * Time.deltaTime);
        if (zDestination == position.z)
        {
            transform.position = new Vector3(position.x, position.y, zStart);
        }
    }
}
