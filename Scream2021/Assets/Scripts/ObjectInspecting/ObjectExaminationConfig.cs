using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExaminationConfig : MonoBehaviour { 

    [SerializeField] float distanceFromCam = 2;
    [SerializeField] bool offsetCamPos = false;
    [SerializeField] float offsetY = 1f;
    [SerializeField] float offsetX = 1f;


    public float ReturnDistanceFromCam() { return distanceFromCam; }
    public bool ReturnIfOffset() { return offsetCamPos; }
    public float ReturnXOffset() { return offsetX; }
    public float ReturnYOffset() { return offsetY; }
}
