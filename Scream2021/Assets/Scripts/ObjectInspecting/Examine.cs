
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    [SerializeField] Canvas examineCanvas;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject player;
    Camera mainCam;//Camera Object Will Be Placed In Front Of
    GameObject clickedObject;//Currently Clicked Object

    //Holds Original Postion And Rotation So The Object Can Be Replaced Correctly
    Vector3 originaPosition;
    Vector3 originalRotation;
    [SerializeField] float distanceFromCam;


    Transform originalParent;

    //If True Allow Rotation Of Object
    public bool examineMode;

    void Start()
    {
        mainCam = Camera.main;
        examineMode = false;


    }

    private void Update()
    {

        ClickObject();//Decide What Object To Examine

        TurnObject();//Allows Object To Be Rotated
        Debug.Log("examine mode " + examineMode); 
        if (examineMode && !dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            ExitExamineMode();
        }


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

                if (clickedObject.tag == ("Selected") && clickedObject.GetComponent<ObjectExaminationConfig>() && Input.GetKeyDown(KeyCode.E)) 
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; 
                    examineMode = true;
                    Debug.Log("examineMode"); 
                    GetComponent<MouseLook>().LockCamera();
                    clickedObject.GetComponent<Selectable>().DisableSelectable();
                    FindObjectOfType<PlayerMovement>().LockPlayer();

                    distanceFromCam = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnDistanceFromCam();

                    examineCanvas.gameObject.SetActive(true);
                    //Save The Original Postion And Rotation
                    originaPosition = clickedObject.transform.position;
                    originalRotation = clickedObject.transform.rotation.eulerAngles;

                    //Now Move Object In Front Of Camera, offsets if bool is true
                    var relativePosition = mainCam.transform.InverseTransformDirection(transform.position - mainCam.transform.position);
                    clickedObject.transform.position = transform.position + (transform.forward * distanceFromCam);

                    //checking if this object has config script and if the position should be offset, if true offset according to it's script.
                    if (clickedObject.GetComponent<ObjectExaminationConfig>())
                    {
                        if (clickedObject.GetComponent<ObjectExaminationConfig>().ReturnIfOffset() == true)
                        { 
                            var xOffset = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnXOffset();
                            var yOffset = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnYOffset();


                            clickedObject.transform.position += (transform.right * xOffset);
                            clickedObject.transform.position += (transform.up * yOffset);
                        }
                        float xRotation = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnXRotation();
                        float yRotation = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnYRotation();
                        float zRotation = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnZRotation(); 

                        clickedObject.transform.rotation = 
                            Quaternion.Euler(xRotation, yRotation, zRotation); 

                    }
                    //Pause The Game
                    /* Time.timeScale = 0; */

                    //Turn Examine Mode To True
                    

                    player.tag = ("Untagged");

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

            Vector3 centerPosition = clickedObject.GetComponent<Renderer>().bounds.center;

            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            clickedObject.transform.RotateAround(centerPosition, Camera.main.transform.up, -xAxis);
            clickedObject.transform.RotateAround(centerPosition, Camera.main.transform.right, yAxis);
        }
    }

    public void ExitExamineMode()
    {
        if (examineMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 

            player.tag = ("Player");

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
            
        }
    }
}
