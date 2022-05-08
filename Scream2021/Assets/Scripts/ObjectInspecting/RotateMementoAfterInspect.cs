using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMementoAfterInspect : MonoBehaviour
{
    [SerializeField] Transform transfromToRotate = null;
    [SerializeField] float rotateToY = 45f;
    [SerializeField] float rotateSpeedInSec = 30f;
    [SerializeField] bool spookPlayer = false;
    [SerializeField] float spookRotateMultiplier = 20f;

    float startRotation = 0f;
    bool triggerRotation = false;
    bool triggerSpook = false;
    public bool rotationDone = false;

    private void Start()
    {
        startRotation = transfromToRotate.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerRotation)
        {
            if(transfromToRotate)
            {
                if(triggerSpook == false)
                {
                    Rotate(rotateToY, rotateSpeedInSec);
                }
                else
                {
                    Rotate(startRotation, -spookRotateMultiplier * rotateSpeedInSec);
                }
            }
        }
    }

    private void Rotate(float rotateTo, float speed)
    {
        if ((transfromToRotate.localEulerAngles.y > (rotateTo - 5)) && (transfromToRotate.localEulerAngles.y < (rotateTo + 5)))
        {
            transfromToRotate.localEulerAngles = new Vector3
            (
                transfromToRotate.localEulerAngles.x,
                rotateTo,
                transfromToRotate.localEulerAngles.z
            );
            if(rotateTo == rotateToY)
            {
                if(spookPlayer == true)
                {
                    triggerSpook = true;
                }
                else
                {
                    FindObjectOfType<Examine>().turningObjectEnabled = true;
                    triggerRotation = false;
                    rotationDone = true;
                }
            }
            else
            {
                FindObjectOfType<Examine>().turningObjectEnabled = true;
                triggerRotation = false;
                rotationDone = true;
            }
        }
        else
        {
            transfromToRotate.localEulerAngles = new Vector3
            (
                transfromToRotate.localEulerAngles.x,
                transfromToRotate.localEulerAngles.y + speed * Time.deltaTime,
                transfromToRotate.localEulerAngles.z
            );
        }
    }

    public void TriggerRotation()
    {
        triggerRotation = true;
        FindObjectOfType<Examine>().turningObjectEnabled = false;
    }
}
