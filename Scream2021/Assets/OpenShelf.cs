using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShelf : MonoBehaviour, ISaveable
{
    [SerializeField] bool Locked = true;
    [SerializeField] Transform shelfDoor = null;
    [SerializeField] float rotateToAngle = -90;
    [SerializeField] float rotateSpeedInSec = -90;
    [SerializeField] GameObject[] objectsToEnable = null;
    int interactionCounter = 0;

    private void Start()
    {
        if (rotateToAngle < 0)
        {
            rotateToAngle = 360 - Mathf.Abs(rotateToAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }

        Rotate();
    }

    bool rotateToOpen = false;
    private void Interaction()
    {
        interactionCounter++;
        if(Locked == false)
        {
            rotateToOpen = true;
            foreach(GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
        }
    }

    private void Rotate()
    {
        if(rotateToOpen)
        {
            Debug.Log(rotateToAngle);
            Debug.Log(shelfDoor.localEulerAngles.y);
            if((shelfDoor.localEulerAngles.y <= rotateToAngle + 10f) && (shelfDoor.localEulerAngles.y >= rotateToAngle - 10f))
            {
                shelfDoor.localEulerAngles = new Vector3(
                    shelfDoor.localEulerAngles.x,
                    rotateToAngle,
                    shelfDoor.localEulerAngles.z
                    );
            }
            else
            {
                shelfDoor.localEulerAngles = new Vector3(
                    shelfDoor.localEulerAngles.x,
                    shelfDoor.localEulerAngles.y + rotateSpeedInSec * Time.deltaTime,
                    shelfDoor.localEulerAngles.z
                    );

                if ((shelfDoor.localEulerAngles.y <= rotateToAngle + 5f) && (shelfDoor.localEulerAngles.y >= rotateToAngle - 5f))
                {
                    shelfDoor.localEulerAngles = new Vector3(
                        shelfDoor.localEulerAngles.x,
                        rotateToAngle,
                        shelfDoor.localEulerAngles.z
                        );
                }
            }
        }
    }

    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
        public bool rotateToOpen;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        data.rotateToOpen = rotateToOpen;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;
        rotateToOpen = data.rotateToOpen;
        if (rotateToOpen)
        {
            shelfDoor.localEulerAngles = new Vector3
                (
                shelfDoor.localEulerAngles.x,
                rotateToAngle,
                shelfDoor.localEulerAngles.z
                );
        }
    }
}
