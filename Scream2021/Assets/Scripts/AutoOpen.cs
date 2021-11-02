using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOpen : MonoBehaviour
{ 
    [SerializeField] float pushDistance;
    [SerializeField] bool negative = false;
// Start is called before the first frame update
void Start()
{

}

// Update is called once per frame
void Update()
{
    
}
 public void PushDoor()
{
        if (negative) { pushDistance *= -1f; }
    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - pushDistance);
        pushDistance = 0;   
}

}
