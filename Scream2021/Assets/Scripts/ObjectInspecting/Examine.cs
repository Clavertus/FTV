
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{
    //[SerializeField]
    Canvas examineCanvas;
    //[SerializeField]
    GameObject dialogueBox;
    [SerializeField] GameObject playerBody;
    [SerializeField] GameObject player;
    [SerializeField] Transform cameraFrontObject;//Camera Object Will Be Placed In Front Of
    GameObject clickedObject;//Currently Clicked Object

    //Holds Original Postion And Rotation So The Object Can Be Replaced Correctly
    Vector3 originaPosition;
    Vector3 originalRotation;
    [SerializeField] float distanceFromCam; 
    //[SerializeField] bool useCalculatedCenterPosition;


    Transform originalParent;

    //If True Allow Rotation Of Object
    private bool examineMode;

    void Start()
    {
        examineCanvas = FindObjectOfType<ExamineCanvas>().GetComponent<Canvas>();
        dialogueBox = FindObjectOfType<DialogueUI>().dialogueBox;
        examineMode = false;
    }

    private bool examineModeToogle = false;
    private void Update()
    {

        ClickObject();//Decide What Object To Examine

        if(examineMode == true)
        {
            cameraFrontObject.GetComponent<MouseLook>().LockCamera();
            player.GetComponent<PlayerMovement>().LockPlayer();
            FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        }
    }

    private void LateUpdate()
    {
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
                //Debug.LogError(clickedObject.name);

                ObjectExaminationConfig examineConfig = clickedObject.GetComponent<ObjectExaminationConfig>();
                if (
                    (clickedObject.tag == ("Selected")) && examineConfig &&
                    (((examineConfig.extraPressToShow == true) && Input.GetKeyDown(KeyCode.E)) || (examineConfig.extraPressToShow == false))
                    ) 
                {
                    examineMode = true;
                    Debug.Log("examineMode");
                    cameraFrontObject.GetComponent<MouseLook>().LockCamera();
                    clickedObject.GetComponent<Selectable>().DisableSelectable();
                     
                    FindObjectOfType<PlayerMovement>().LockPlayer();
                    
                    distanceFromCam = clickedObject.GetComponent<ObjectExaminationConfig>().ReturnDistanceFromCam();

                    examineCanvas.enabled = true;
                    //FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(false);
                    //Save The Original Postion And Rotation
                    originaPosition = clickedObject.transform.position;
                    originalRotation = clickedObject.transform.rotation.eulerAngles;

                    //Now Move Object In Front Of Camera, offsets if bool is true
                    var relativePosition = cameraFrontObject.transform.InverseTransformDirection(transform.position - cameraFrontObject.transform.position);
                    clickedObject.transform.position = transform.position + (transform.forward * distanceFromCam);
                    savedCenterPosition = clickedObject.transform.position;

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

                        savedObjectPivotPosition = clickedObject.transform.position;

                    }
                    //Pause The Game
                    /* Time.timeScale = 0; */

                    //Turn Examine Mode To True
                    

                    playerBody.tag = ("Untagged");
                }
            }
        }
    }

    public bool GetExamineMode()
    {
        return examineMode;
    }

    public bool turningObjectEnabled = true; 
    Vector3 savedCenterPosition;
    Vector3 savedObjectPivotPosition;
    void TurnObject()
    {
        if (Input.GetMouseButton(0) && examineMode)
        {
            if (turningObjectEnabled == false)
            {
                //do not allow to rotate while dialog
                return;
            }

            /*if (dialogueBox.activeSelf)
            {
                //do not allow to rotate while dialog
                return;
            }*/

            clickedObject.tag = ("Untagged");
            
            float rotationSpeed = 15 * clickedObject.GetComponent<ObjectExaminationConfig>().ReturnTurnSpeedModifier();

            Vector3 rotatePosition = savedObjectPivotPosition;

            float xAxis = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotationSpeed;

            

            clickedObject.transform.RotateAround(rotatePosition, Camera.main.transform.up, -xAxis);
            clickedObject.transform.RotateAround(rotatePosition, Camera.main.transform.right, yAxis);
            clickedObject.transform.position = savedObjectPivotPosition;
        }
    }

    public void ExitExamineMode()
    {
        if (examineMode)
        {
            examineCanvas.enabled = false;
            FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(false);
            playerBody.tag = ("Player");

            cameraFrontObject.GetComponent<MouseLook>().UnlockCamera();
            FindObjectOfType<PlayerMovement>().UnlockPlayer();
            FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();

            examineCanvas.enabled = false;
            FindObjectOfType<ExamineCanvas>().SetExtraFieldToState(false);
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
