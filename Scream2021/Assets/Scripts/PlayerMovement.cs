using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float gravity = -9.81f;

    [Header("Ground Check")]
    //groundDistance is size of the layerMask. 
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask; 

    Vector3 velocity;
    bool isGrounded; 
    
    void Update()
    {
        Move();
        PlayerGravity();
    }

    private void Move()
    {
        //get the negative or positive x and z player inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //make vector3 using directions relative to where player is facing, times the player inputs
        Vector3 move = transform.right * x + transform.forward * z;

        //let character controller do all the work from here ;)
        controller.Move(move * moveSpeed * Time.deltaTime);  
    }

    private void PlayerGravity()
    {
        //because it checks with layermask, make sure all objects the player can stand on are of layer "Ground"
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 

        //reset velocity so it doesn't infinetely count
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        //increment our velocity variable with the gravity variable then let character controller do the rest ;)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
