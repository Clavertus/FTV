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
    bool movementLock;
    private void Start()
    {
        movementLock = false; 
    }
    void Update()
    {
        LockerTimer();
        Move();
        PlayerGravity();
    }

    public void LockPlayer() { movementLock = true; }
    public void UnlockPlayer() { movementLock = false; }


    [SerializeField] float FootstepPlayRate = 2f;
    float FootstepCntTime = 0f;
    private void Move()
    {
        if (movementLock) { return; }
        //get the negative or positive x and z player inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //make vector3 using directions relative to where player is facing, times the player inputs
        Vector3 move = transform.right * x + transform.forward * z;

        //let character controller do all the work from here ;)
        controller.Move(move * moveSpeed * Time.deltaTime);

        if((Mathf.Abs(x) > Mathf.Epsilon) || (Mathf.Abs(z) > Mathf.Epsilon))
        {
            FootstepPlaying();
        }
        else
        {
            AudioManager.instance.InstantStopFromAudioManager(soundsEnum.Footstep1);
        }
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

    private void FootstepPlaying()
    {
        if (FootstepPlayRate <= FootstepCntTime)
        {
            FootstepCntTime = 0f;
            AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Footstep1);
        }
        else
        {
            FootstepCntTime += Time.deltaTime;
        }
    }

    float lockPlayerTime = 0f;
    float lockPlayerTimer = 0f;
    bool lockPlayerTimerEnabled = false;
    public void LockPlayerForTime(float effectTime)
    {
        lockPlayerTime = effectTime;
        lockPlayerTimer = 0f;
        LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
    }

    private void LockerTimer()
    {
        if((movementLock) && (lockPlayerTimerEnabled))
        {
            if (lockPlayerTimer > lockPlayerTime)
            {
                UnlockPlayer();
                GetComponent<MouseLook>().UnlockCamera();
                lockPlayerTimerEnabled = false;
            }
            else
            {
                lockPlayerTimer += Time.deltaTime;
            }
        }
    }
}
