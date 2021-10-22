using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    [SerializeField] Canvas examineCanvas;
    Camera mainCam;//Camera Object Will Be Placed In Front Of
    GameObject clickedObject;//Currently Clicked Object

    //Holds Original Postion And Rotation So The Object Can Be Replaced Correctly
    Vector3 originaPosition;
    Vector3 originalRotation;
    [SerializeField] float distanceFromCam;


    //If True Allow Rotation Of Object
    bool examineMode;

    void Start()
    {
        mainCam = Camera.main;
        examineMode = false;


    }

    private void Update()
    {

        ClickObject();//Decide What Object To Examine

        TurnObject();//Allows Object To Be Rotated




    }


    void ClickObject()
    {
        if (examineMode == false)
        {

            RaycastHit hit;
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));


            if (Physics.Raycast(ray, out hit))
            {

                //ClickedObject Will Be The Object Hit By The Raycast
                clickedObject = hit.transform.gameObject;

                if (clickedObject.GetComponent<Memento>() && Input.GetKeyDown(KeyCode.E))  
                {
                    GetComponent<MouseLook>().LockCamera();
                    clickedObject.GetComponent<Selectable>().DisableSelectable();
                    FindObjectOfType<PlayerMovement>().LockPlayer();


                    examineCanvas.gameObject.SetActive(true);
                    //Save The Original Postion And Rotation
                    originaPosition = clickedObject.transform.position;
                    originalRotation = clickedObject.transform.rotation.eulerAngles;

                    //Now Move Object In Front Of Camera
                    clickedObject.transform.position = mainCam.transform.position + (transform.forward * distanceFromCam);

                    //Pause The Game
                    /* Time.timeScale = 0; */

                    //Turn Examine Mode To True
                    examineMode = true;

                }
            }
        }
    }
    
    void TurnObject()
    {
        if (Input.GetMouseButton(0) && examineMode)
        {
           clickedObject.tag = ("Untagged");
           examineCanvas.gameObject.SetActive(false);
           float rotationSpeed = 15;

            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            clickedObject.transform.Rotate(Vector3.up, -xAxis,  Space.World);
            clickedObject.transform.Rotate(Vector3.forward, yAxis, Space.World);
        }
    }

    public void ExitExamineMode()
    {
        if (examineMode)
        {
            GetComponent<MouseLook>().UnlockCamera();
            FindObjectOfType<PlayerMovement>().UnlockPlayer();

            examineCanvas.gameObject.SetActive(false);
            //Reset Object To Original Position
            clickedObject.transform.position = originaPosition;
            clickedObject.transform.eulerAngles = originalRotation;


            //Unpause Game
            /*Time.timeScale = 1; */

            //Return To Normal State
            examineMode = false;
            FindObjectOfType<GameboyMemento>().ExitedExamineMode(); 
        }
    }
}
