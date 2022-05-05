using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTrain : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;

    [SerializeField] Transform pointA = null;
    [SerializeField] Transform pointB = null;
    [SerializeField] Transform pointC = null;

    [SerializeField] float speed = 10f;

    [Header("Train speed used to emulate look of the train catching up")]
    [SerializeField] float minSpeedChangeTimer = 1f;
    [SerializeField] float maxSpeedChangeTimer = 3f;
    float speedChangeTimer; 

    [SerializeField] float currentCatchUpSpeed = 3f;
    [SerializeField] float maxCatchUpSpeed = 5f;
    [SerializeField] float minCatchUpSpeed = 1f;

    [SerializeField] float minRandomSpeedChange = -1f;
    [SerializeField] float maxRandomSpeedChange = 1f;

    [SerializeField] float minSpeedDip = .2f;
    [SerializeField] float maxSpeedDip = 1f;

    [SerializeField] float minSpeedSpike = .2f;
    [SerializeField] float maxSpeedSpike = 1f;

    bool hitPointB = false;
    bool triggerTrain = false; 
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
            transform.rotation = Quaternion.Euler(0, 90, 0);   
            
        }
        if (hitPointB && triggerTrain) 
        {
            CatchUpSpeed();
            MoveToPointC();
        }
    }

    public void SetPointBPosition()
    {
        transform.position = pointB.transform.position;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void TriggerTrain()
    {
        triggerTrain = true; 
    }
    private void MoveToPointB()
    {
        transform.position = Vector3.MoveTowards(pointA.position, pointB.position, speed * Time.deltaTime);
    }

    private void CatchUpSpeed()
    {
        if (speedChangeTimer <= 0)
        {
            currentCatchUpSpeed += Random.Range(minRandomSpeedChange, maxRandomSpeedChange); 
            speedChangeTimer = Random.Range(minSpeedChangeTimer, maxSpeedChangeTimer); 
        }
        speedChangeTimer -= Time.deltaTime;

        
        if(currentCatchUpSpeed >= maxCatchUpSpeed) 
        { currentCatchUpSpeed -= Random.Range(minSpeedDip, maxSpeedDip);  }

        else if (currentCatchUpSpeed <= minCatchUpSpeed) 
        { currentCatchUpSpeed += Random.Range(minSpeedSpike, maxSpeedSpike); }
        Debug.Log(currentCatchUpSpeed); 
    }
    private void MoveToPointC()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointC.transform.position, currentCatchUpSpeed * Time.deltaTime); 
    }
    public void increaseSpeed()
    {
        speed = currentCatchUpSpeed;
    }
    public void OnEnable()
    {
        enableMovement = true;
        door1.GetComponent<AutoOpen>().PushDoor();
        door2.GetComponent<AutoOpen>().PushDoor();
    }

    private void OnDisable()
    {
        enableMovement = false;
    }
}
