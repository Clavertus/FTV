using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondarySelctable : MonoBehaviour
{
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        Debug.Log("test");  
    }

    private void OnMouseExit()
    {
        gameObject.transform.position *= -2;
         
    }
}
