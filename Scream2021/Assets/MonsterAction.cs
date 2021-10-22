using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    public enum monsterStatesEnm
    {
        idle,
        walk,
        reveal,
        run,
        to_open,
        in_open,
        final_open,
        walk_after_open,
        action,
        run_to_player,
        jump_and_kill
    };

    [SerializeField] Animator myAnimator = null;
    [SerializeField] monsterStatesEnm currentState = monsterStatesEnm.idle;
    monsterStatesEnm lastState = monsterStatesEnm.idle;

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float pauseAfterReveal = 1f;
    [SerializeField] float walkAfterDoorSpeed = 4f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    [SerializeField] Transform revealZone = null;
    bool revealZoneTriggered = false;
    [SerializeField] Transform doorZone = null;
    bool doorZoneTriggered = false;
    [SerializeField] Transform actionZone = null;
    bool actionZoneTriggered = false;
    [SerializeField] Transform Player = null;

    public bool StartSequence = false;
    [SerializeField] int numberOfTriesToOpenDoor = 5;

    [SerializeField] float jumpDistance = 20f;
    bool InJump = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!myAnimator)
        {
            Debug.LogError("Monster need animation controller to work!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastState != currentState)
        {
            lastState = currentState;
            TriggerAnimationWithId((int)currentState);
        }

        AnimationStateMachine();
    }

    private void TriggerAnimationWithId(int currentState)
    {
        myAnimator.SetTrigger(currentState.ToString());
    }


    private void OnEnable()
    {
        //TODO: set this variable to true from another script after player has done final things
        StartSequence = true;
    }

    float timeCounter = 0f;
    private void AnimationStateMachine()
    {
        switch(currentState)
        {
            case monsterStatesEnm.idle:
                //simply do nothing and wait to be triggered
                if (StartSequence)
                {
                    currentState = monsterStatesEnm.walk;
                }
                break;
            case monsterStatesEnm.walk:
                MonsterMove(walkSpeed);
                if (revealZoneTriggered)
                {
                    currentState = monsterStatesEnm.reveal;
                }
                break;
            case monsterStatesEnm.reveal:
                if (timeCounter >= pauseAfterReveal)
                {
                    currentState = monsterStatesEnm.run;
                }
                timeCounter += Time.deltaTime;
                break;
            case monsterStatesEnm.run:
                MonsterMove(runSpeed);
                if (doorZoneTriggered)
                {
                    currentState = monsterStatesEnm.to_open;
                }
                break;
            case monsterStatesEnm.to_open:
                if (finishedToOpen)
                {
                    currentState = monsterStatesEnm.in_open;
                }
                break;
            case monsterStatesEnm.in_open:
                if(finishedInOpen >= numberOfTriesToOpenDoor)
                {
                    currentState = monsterStatesEnm.final_open;
                }
                break;
            case monsterStatesEnm.final_open:
                if(finishedFinallyOpen)
                {
                    currentState = monsterStatesEnm.walk_after_open;
                }
                break;
            case monsterStatesEnm.walk_after_open:
                MonsterMove(walkAfterDoorSpeed);
                if (actionZoneTriggered)
                {
                    currentState = monsterStatesEnm.action;
                }
                break;
            case monsterStatesEnm.action:
                if (finishedAction)
                {
                    currentState = monsterStatesEnm.run_to_player;
                }
                break;
            case monsterStatesEnm.run_to_player:
                //move to jump positions between monster and player (Z) 
                MonsterMove(runSpeed);
                if(Vector3.Distance(transform.position, Player.position ) <= jumpDistance)
                {
                    currentState = monsterStatesEnm.jump_and_kill;
                }
                break;
            case monsterStatesEnm.jump_and_kill:
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("InJump") && !InJump)
                {
                    transform.LookAt(Player);
                    InJump = true;
                }

                if(InJump)
                {
                    MonsterJump();
                    if(PlayerFound)
                    {
                        BadEndGameTrigger();
                    }
                }
                else
                {
                    MonsterMove(runSpeed);
                }
                break;
            default:
                break;
        }
    }
    private void MonsterJump()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, Player.position, jumpSpeed * Time.deltaTime);

        newPosition = new Vector3(newPosition.x, transform.position.y, newPosition.z);

        transform.position = newPosition;
    }

    private void MonsterMove(float speed)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
    }

    private void BadEndGameTrigger()
    {
        Debug.Log("Game has been ended with BAD END! :-)");
        //make something to end the game
        //currently just disables monster
        this.gameObject.SetActive(false);
    }

    bool PlayerFound = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            if (other.transform == revealZone)
            {
                revealZoneTriggered = true;
            }
            if (other.transform == doorZone)
            {
                doorZoneTriggered = true;
            }
            if (other.transform == actionZone)
            {
                actionZoneTriggered = true;
            }
            if(other.transform == Player)
            {
                PlayerFound = true;
            }
        }
    }

    bool finishedToOpen = false;
    // This C# function can be called by an Animation Event
    public void FinishedToOpen()
    {
        finishedToOpen = true;
    }

    int finishedInOpen = 0;
    // This C# function can be called by an Animation Event
    public void FinishedInOpen()
    {
        finishedInOpen++;
    }

    bool finishedFinallyOpen = false;
    // This C# function can be called by an Animation Event
    public void FinallyOpenFinished()
    {
        finishedFinallyOpen = true;
    }

    bool finishedAction = false;
    // This C# function can be called by an Animation Event
    public void ActionFinished()
    {
        finishedAction = true;
    }
}
