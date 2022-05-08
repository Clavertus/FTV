using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOpen : MonoBehaviour
{ 
    [SerializeField] float pushDistance;
    [SerializeField] bool negative = false;
    [SerializeField] bool xAxisPush = false;

    public void PushDoor()
    {
        if (negative) { pushDistance *= -1f; }

        if(!xAxisPush)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + pushDistance);
        }else
        {
            gameObject.transform.position = new Vector3(transform.position.x + pushDistance, transform.position.y, transform.position.z);
        }

        pushDistance = 0;
    }

}
