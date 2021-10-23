using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTrain : MonoBehaviour
{
    [SerializeField] Transform pointA = null;
    [SerializeField] Transform pointB = null;
    [SerializeField] Transform pointC = null;

    [SerializeField] float speed = 10f;
    [SerializeField] float speedBoost = 10f;
    bool hitPointB = false;
    bool placedLastMemento = false; 
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
        if ((pointB != null) && enableMovement && !hitPointB) 
        {
            MoveToPointB();
        }
        
        if (transform.position == pointB.transform.position)
        {
            Debug.Log("hit");
            hitPointB = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);   
            
        }
        if (hitPointB && placedLastMemento) 
        {
            Debug.Log("moveC");
            MoveToPointC();
        }
    }

    public void IsLastMementoPlaced()
    {
        placedLastMemento = true; 
    }
    private void MoveToPointB()
    {
        transform.position = Vector3.MoveTowards(pointA.position, pointB.position, speed * Time.deltaTime);
    }
    private void MoveToPointC()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointC.transform.position, speedBoost * Time.deltaTime); 
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
