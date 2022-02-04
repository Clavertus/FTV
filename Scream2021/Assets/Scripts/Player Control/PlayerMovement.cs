using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float runRate = 1.5f;
    [SerializeField] private bool runEnable = false;
    [SerializeField] float gravity = -9.81f;

    public void SetRunEnable(bool newValue)
    {
        runEnable = newValue;
    }

    [SerializeField] float FootstepPlayRate = 2f;
    float FootstepCntTime = 0f;

    [Header("Ground Check")]
    //groundDistance is size of the layerMask. 
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool movementLock = false;

    private void Start()
    {
        //movementLock = false; 
    }

    void Update()
    {
        Move();
        PlayerGravity();
        SimulateFall();
    }

    public void LockPlayer() { movementLock = true; }
    public void UnlockPlayer() { movementLock = false; }


    private void Move()
    {
        if (movementLock) { return; }
        //get the negative or positive x and z player inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //make vector3 using directions relative to where player is facing, times the player inputs
        Vector3 move = transform.right * x + transform.forward * z;

        float speed, stepPlayRate;
        DetermineStepOrRunSpeed(out speed, out stepPlayRate);

        //let character controller do all the work from here ;)
        controller.Move(move * speed * Time.deltaTime);

        if ((Mathf.Abs(x) > Mathf.Epsilon) || (Mathf.Abs(z) > Mathf.Epsilon))
        {
            FootstepPlaying(stepPlayRate);
        }
    }

    private void DetermineStepOrRunSpeed(out float speed, out float stepPlayRate)
    {
        speed = moveSpeed;
        stepPlayRate = FootstepPlayRate;
        if (Input.GetKey(KeyCode.LeftShift) && runEnable)
        {
            speed = moveSpeed * runRate;
            stepPlayRate = FootstepPlayRate / runRate;
        }
    }

    private void PlayerGravity()
    {
        //because it checks with layermask, make sure all objects the player can stand on are of layer "Ground"
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 

        //reset velocity so it doesn't infinetely count
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        //increment our velocity variable with the gravity variable then let character controller do the rest ;)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private bool simulateFallEnabled = false;
    private void SimulateFall()
    {
        if(simulateFallEnabled && (gravity <= Mathf.Epsilon))
        {
            controller.Move(new Vector3(0, -9.7f, 0) * Time.deltaTime);
        }
    }

    public void MakeSimulateFall()
    {
        simulateFallEnabled = true;
    }

    private void FootstepPlaying(float playRate)
    {
        if (playRate <= FootstepCntTime)
        {
            FootstepCntTime = 0f;
            int footstepType = (int) soundsEnum.Footstep1;
            AudioManager.instance.PlayOneShotFromAudioManager((soundsEnum)footstepType);
        }
        else
        {
            FootstepCntTime += Time.deltaTime;
        }
    }
}
