using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;

    //these are the upper and lower boundaries of the camera
    [SerializeField] int lowLookClamp = -90;
    [SerializeField] int highLookClamp = 90;

    [SerializeField] Transform playerBody;

    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

    } 
    private void LateUpdate()
    {
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
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotate around y axis for horizontal looking
        playerBody.Rotate(Vector3.up * mouseX);
        

    }
}
