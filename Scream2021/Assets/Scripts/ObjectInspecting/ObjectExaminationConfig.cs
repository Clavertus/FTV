using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExaminationConfig : MonoBehaviour { 

    [SerializeField] float distanceFromCam = 2;
    [SerializeField] bool offsetCamPos = false;
    [SerializeField] float offsetY = 1f;
    [SerializeField] float offsetX = 1f;

    [SerializeField] float xRotation = 0;
    [SerializeField] float yRotation = 0;
    [SerializeField] float zRotation = 0;
    [SerializeField] public bool extraPressToShow = true;

    public float ReturnDistanceFromCam() { return distanceFromCam; }
    public bool ReturnIfOffset() { return offsetCamPos; }
    public float ReturnXOffset() { return offsetX; }
    public float ReturnYOffset() { return offsetY; } 

    public float ReturnXRotation() { return xRotation; }
    public float ReturnYRotation() { return yRotation; }
    public float ReturnZRotation() { return zRotation; }


    

}
