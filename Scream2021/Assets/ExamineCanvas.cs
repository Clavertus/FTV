using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineCanvas : MonoBehaviour
{
    //this is used as a type
    private void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}
