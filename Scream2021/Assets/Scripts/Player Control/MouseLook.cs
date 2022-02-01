using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 100f;

    //these are the upper and lower boundaries of the camera
    [SerializeField] int lowLookClamp = -90;
    [SerializeField] int highLookClamp = 90;
    [SerializeField] float lookOffset = 3f; 
    [SerializeField] Transform playerBody;

    bool lockCamera = false;
    bool monsterJumping = false;

    bool cameraCorrected = false;

    float camXrot;

    float xRotation = 0f;

    void Start()
    {
        if (PlayerPrefs.HasKey("mouse_sensivity")) mouseSensitivity = PlayerPrefs.GetFloat("mouse_sensivity");

        // Cursor.lockState = CursorLockMode.Locked;

        /*
        if (PlayerPrefs.HasKey("camXrot"))
        {
            camXrot = PlayerPrefs.GetFloat("camXrot");
            Camera.main.transform.eulerAngles = new Vector3(camXrot, 0, 0);
            cameraCorrected = true;
        }
        */
    }

    public void LockCamera() { lockCamera = true; }
    public void UnlockCamera() { lockCamera = false; }


    private void LateUpdate()
    {
        if (lockCamera) { return; }
        LookWithMouse();
    }

    private void LookWithMouse()
    {
        //getting that mouse x and y movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotate around the x axis for vertical looking. rotate the camera instead of player.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lowLookClamp, highLookClamp);
       
            transform.localRotation = Quaternion.Euler(xRotation + camXrot, 0f, 0f);
        

        //rotate around y axis for horizontal looking
        playerBody.Rotate(Vector3.up * mouseX);       
    }

    public void MonsterIsJumping() { 
        monsterJumping = true;
        LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
        var monsterTransform = FindObjectOfType<MonsterAction>().gameObject.transform.position;
        transform.LookAt(new Vector3(monsterTransform.x, monsterTransform.y + lookOffset, monsterTransform.z));
    }

    public void LockAndLookAtPoint(Vector3 lookAtPosition)
    {
        LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
        transform.LookAt(lookAtPosition);
    }

    public void UnlockFromPoint()
    {
        UnlockCamera();
        FindObjectOfType<PlayerMovement>().UnlockPlayer();
    }


}
