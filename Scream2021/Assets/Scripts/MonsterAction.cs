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
        jump_and_kill,
        chew,
        end_of_state
    };

    [SerializeField] Animator myAnimator = null;
    [SerializeField] Light myExtraLight = null;

    [Header("State machine feedback")]
    [SerializeField] monsterStatesEnm currentState = monsterStatesEnm.idle;
    monsterStatesEnm lastState = monsterStatesEnm.idle;

    [Header("Speed controls")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float walkAfterDoorSpeed = 4f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    [Header("Gameplay controls")]
    [SerializeField] float pauseAfterReveal = 1f;
    [SerializeField] int numberOfTriesToOpenDoor = 5;
    [SerializeField] float jumpDistance = 20f; 
    [SerializeField] float lookAtDistance = 30f;
    [SerializeField] float minDistance = 4f;
    [SerializeField]
    float playerFoundDistance = 4f;
    [SerializeField] Transform monsterInTheDoor; 

    [Header("References to transforms")]
    [SerializeField] Transform revealZone = null;
    bool revealZoneTriggered = false;
    [SerializeField] Transform doorZone = null;
    [SerializeField] Transform doorStayPoint = null;
    bool doorZoneTriggered = false;
    [SerializeField] Transform actionZone = null;
    bool actionZoneTriggered = false;
    [SerializeField] Transform Player = null;

    [Header("Player Dialogue")]
    [SerializeField] GameObject dialogueBox; 
    [SerializeField] DialogueObject gTFO;



    [Header("Change StartSequence to true to start monster final sequence")]
    [Header("TODO: it is also now triggered by OnEnable (CHANGE THIS!)")]
    public bool StartSequence = false;

    bool InJump = false;

    public AudioSource monsterBreathe;
    public AudioSource monsterAttack;
    public AudioSource monsterAgressive;
    public AudioSource monsterAgressive2; 
    public AudioSource monsterFootstep;
    public AudioSource monsterForceDoor1;
    public AudioSource monsterForceDoor2;



    // Start is called before the first frame update
    void Start()
    {
        if(!myAnimator)
        {
            Debug.LogError("Monster need animation controller to work!");
        }

        if(!revealZone)
        {
            Debug.LogError("reveal zone must be applied!");
        }

        if (!doorZone)
        {
            Debug.LogError("door zone must be applied!");
        }

        if (!actionZone)
        {
            Debug.LogError("action zone must be applied!");
        }

        if (!Player)
        {
            Debug.LogError("player transform must be applied!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        FadeOutLight();

        if(lastState != currentState)
        {
            lastState = currentState;
            TriggerAnimationWithId((int)currentState);
        }

        StartCoroutine(AnimationStateMachine());
    }

    private void FadeOutLight()
    {
        if(myExtraLight)
        {
            if(myExtraLight.intensity < 0.35f)
            {
                myExtraLight.intensity += 0.5f * Time.deltaTime;
            }
        }
    }

    private void TriggerAnimationWithId(int currentState)
    {
        myAnimator.SetTrigger(currentState.ToString());
    }


    private void OnEnable()
    {
        if(myExtraLight)  myExtraLight.intensity = 0f;
           //TODO: set this variable to true from another script after player has done final things
           StartSequence = true;

         monsterBreathe = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterBreathe);
         monsterAttack = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterAttack);
         monsterAgressive = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterMildAggressive);
         monsterAgressive2 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterMildAggressive2);
         monsterFootstep = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.HeavyFootstep1);
         monsterForceDoor1 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.ForceDoor1);
         monsterForceDoor2 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.ForceDoor2);
    }

    public void StopMoving()
    {
        currentState = monsterStatesEnm.idle;
        StartSequence = false;
        
    }
    public void MonsterInTheDoor()
    {
        transform.position = monsterInTheDoor.position;
        transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
    }

    [SerializeField] float offsetJumpY = 2f;
    float offsetY = 0f; 
    float timeCounter = 0f;
    private IEnumerator AnimationStateMachine()
    {
        switch(currentState)
        {
            case monsterStatesEnm.idle:
                //simply do nothing and wait to be triggered
                if (StartSequence)
                {
                    currentState = monsterStatesEnm.walk;
                    AudioManager.instance.InstantPlayFromGameObject(monsterBreathe);  
                }
                break;
            case monsterStatesEnm.walk:
                MonsterMove(walkSpeed);
                if (revealZoneTriggered)
                {
                    yield return new WaitUntil(() => !dialogueBox.activeSelf);
                    currentState = monsterStatesEnm.reveal;
                }
                break;
            case monsterStatesEnm.reveal:

                if (finishedAction)
                {
                    AudioManager.instance.InstantPlayFromGameObject(monsterAgressive);

                    currentState = monsterStatesEnm.run;
                }

                break;
            case monsterStatesEnm.run:
                MonsterMove(runSpeed);
                if (doorZoneTriggered)
                {
                    if (doorStayPoint)
                    {
                        transform.position = doorStayPoint.position;
                        Player.localPosition = Player.localPosition + Vector3.forward * 1.5f;
                    }
                    currentState = monsterStatesEnm.to_open;
                }
                break;
            case monsterStatesEnm.to_open:
                if (finishedToOpen)
                {
                    FindObjectOfType<DialogueUI>().ShowDialogue(gTFO);
                    AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                    FindObjectOfType<SecondDoorToCar>().ShakeDoor();

                    FindObjectOfType<InsideTrainManager>().TriggerSecondDroneSound();

                    currentState = monsterStatesEnm.in_open;
                }
                break;
            case monsterStatesEnm.in_open:
                FlickerLightsInTrainUpdate();
                if (finishedInOpen >= numberOfTriesToOpenDoor)
                {
                    currentState = monsterStatesEnm.final_open;
                }
                break;
            case monsterStatesEnm.final_open:
                FlickerLightsInTrainUpdate();
                if (finishedFinallyOpen)
                {
                    AudioManager.instance.InstantPlayFromGameObject(monsterBreathe);

                    if (Vector3.Distance(transform.position, Player.position) <= minDistance)
                    {
                        MakePlayerLookAtMonster();
                        AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                        currentState = monsterStatesEnm.jump_and_kill;
                    }
                    else
                    {
                        currentState = monsterStatesEnm.run_to_player;
                    }
                }
                break;
            case monsterStatesEnm.walk_after_open:
                FlickerLightsInTrainUpdate();

                if (Vector3.Distance(transform.position, Player.position) <= minDistance)
                {
                    MakePlayerLookAtMonster();
                    AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                    currentState = monsterStatesEnm.jump_and_kill;
                    break;
                }

                MonsterMove(walkAfterDoorSpeed);

                if (actionZoneTriggered)
                {
                    AudioManager.instance.InstantPlayFromGameObject(monsterBreathe);
                    
                    currentState = monsterStatesEnm.action;
                }
                break;
            case monsterStatesEnm.action:
                FlickerLightsInTrainUpdate();

                if (Vector3.Distance(transform.position, Player.position) <= minDistance)
                {
                    MakePlayerLookAtMonster();
                }

                if (finishedAction)
                {
                    AudioManager.instance.InstantPlayFromGameObject(monsterAgressive);
                    AudioManager.instance.InstantPlayFromGameObject(monsterAttack);

                    if (Vector3.Distance(transform.position, Player.position) <= minDistance)
                    {
                        AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                        currentState = monsterStatesEnm.jump_and_kill;
                    }
                    else
                    {
                        currentState = monsterStatesEnm.run_to_player;
                    }
                }
                break;
            case monsterStatesEnm.run_to_player:
                MonsterMove(runSpeed);
                if (Vector3.Distance(transform.position, Player.position) <= lookAtDistance)
                {
                    MakePlayerLookAtMonster();
                }

                if (Vector3.Distance(transform.position, Player.position ) <= jumpDistance)
                {
                    MakePlayerLookAtMonster();
                    currentState = monsterStatesEnm.jump_and_kill;
                }
                break;
            case monsterStatesEnm.jump_and_kill:
                /*
                if (PlayerFound)
                {
                    AudioManager.instance.PlayFromGameObject(monsterAgressive2);
                    BadEndGameTrigger();
                    break;
                }
                */

                CheckDistance();

                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("InJump") && !InJump)
                {
                    InJump = true;
                    AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                }

                if (InJump)
                {
                    if(PlayerFound)
                    {
                        AudioManager.instance.InstantPlayFromGameObject(monsterAgressive2);
                        FindObjectOfType<CameraShaker>().enabled = true;
                        currentState = monsterStatesEnm.chew;
                    }
                    else
                    {
                        MonsterJump();
                    }
                }
                else
                {
                    MonsterMove(runSpeed);
                }
                break;
            case monsterStatesEnm.chew:

                TriggerBloodEffect();


                if(finishedChew)
                {
                    BadEndGameTrigger();
                }
                break;
            default:
                break;
        }
    }

    [SerializeField] GameObject BloodEffectCanvas = null;
    float BloodTimer = 0f;
    private void TriggerBloodEffect()
    {
        if(BloodTimer > 0.25f)
        {
            if(BloodEffectCanvas) BloodEffectCanvas.SetActive(!BloodEffectCanvas.activeSelf);
            BloodTimer = 0f;
            return;
        }
        BloodTimer += Time.deltaTime;
    }

    private void MakePlayerLookAtMonster()
    {
        Debug.Log("LockMenuControl");
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        transform.LookAt(Player);
        FindObjectOfType<MouseLook>().MonsterIsJumping();
        offsetY = transform.position.y + offsetJumpY;
    }

    private void MonsterJump()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, Player.position, jumpSpeed * Time.deltaTime);

        newPosition = new Vector3(newPosition.x, offsetY, newPosition.z);

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

        AudioManager.instance.StopAllSounds();
        //LevelLoader.instance.ending = Ending.Bad;

        //StartCoroutine(FindObjectOfType<SavingWrapper>().CheckpointLoad());
        StartCoroutine(LevelLoader.instance.StartLoadingSameScene(2f));

        currentState = monsterStatesEnm.end_of_state;
    }

    bool PlayerFound = false;

    private void CheckDistance()
    {
        if(!PlayerFound)
        {
            if (Vector3.Distance(transform.position, Player.position) <= playerFoundDistance)
            {
                PlayerFound = true;
            }
        }
    }


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
                //PlayerFound = true;
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
        AudioManager.instance.InstantPlayFromGameObject(monsterBreathe); 

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

    bool finishedChew = false;
    // This C# function can be called by an Animation Event
    public void ChewFinished()
    {
        finishedChew = true;
    }


    // This C# function can be called by an Animation Event
    public void StartOpenDoor()
    {
        FindObjectOfType<SecondDoorToCar>().OpenDoor();
    }

    // This C# function can be called by an Animation Event
    public void ShakeDoor()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterForceDoor2);
        FindObjectOfType<SecondDoorToCar>().ShakeDoor();
    }

    [SerializeField]
    TrainEffectController[] trains;
    private void FlickerLightsInTrain()
    {
        foreach (TrainEffectController train in trains)
        {
            train.FlickerLightForTime(UnityEngine.Random.Range(0.1f, 0.5f));
        }
    }

    float FlickAfterTimeBasic = 1f;
    float FlickAfterTime = 0.25f;
    float FlickTriggerCnt = 0f;
    private void FlickerLightsInTrainUpdate()
    {
        if(FlickAfterTime <= FlickTriggerCnt)
        {
            FlickAfterTime = FlickAfterTimeBasic + UnityEngine.Random.Range(-0.75f, 0.5f);
            FlickTriggerCnt = 0;
            FlickerLightsInTrain();
            return;
        }
        FlickTriggerCnt += Time.deltaTime;
    }

    // This C# function can be called by an Animation Event
    public void Footstep()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterFootstep);
    }

    // This C# function can be called by an Animation Event
    public void ForceDoor()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterForceDoor1);
    }
}
