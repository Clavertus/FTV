using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainMonsterCntrl : MonoBehaviour
{
    public enum monsterStatesEnm
    {
        t_pose,
        reveal,
        walk,
        attack,
        end_of_state
    };

    [SerializeField] Animator myAnimator = null;
    [SerializeField] Transform lookAtPosition = null;
    [SerializeField] Transform playerTransform = null;
    [SerializeField] GameObject myCamera = null;

    private monsterStatesEnm currentState = monsterStatesEnm.t_pose;
    private monsterStatesEnm lastState = monsterStatesEnm.t_pose;
    public AudioSource monsterFootstep;
    private AudioSource monsterSound0;
    private AudioSource monsterSound1;
    private AudioSource monsterScream;
    private AudioSource monsterImpact;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float minimalDistanceToPlayer = 5f;

    internal void MonsterSound0()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterSound0);
    }

    internal void MonsterSound1()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterSound1);
    }

    internal void Impact()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterImpact);
    }

    internal void Scream()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterScream);
    }

    public void Footstep()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterFootstep);
    }

    bool finishedChew = false;
    public void FinishAttack()
    {
        finishedChew = true;
    }

    private void OnEnable()
    {
        monsterFootstep = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.HeavyFootstep1);
        monsterSound0 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterMildAggressive);
        monsterSound1 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterAttack);
        monsterScream = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterBreathe);
        monsterImpact = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.OpenDoor);
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera.SetActive(false);
        int idle = (int) monsterStatesEnm.t_pose;
        myAnimator.SetTrigger(((int)idle).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(lastState != currentState)
        {
            myAnimator.SetTrigger(((int)currentState).ToString());
        }

        lastState = currentState;

        switch (currentState)
        {
            case monsterStatesEnm.t_pose:
                break;
            case monsterStatesEnm.walk:
                MonsterMove(runSpeed);

                if (Vector3.Distance(transform.position, playerTransform.position) <= minimalDistanceToPlayer)
                {
                    currentState = monsterStatesEnm.attack; 
                    MakePlayerLookAtMonster();
                }

                break;
            case monsterStatesEnm.attack:
                TriggerBloodEffect();

                if (finishedChew)
                {
                    BadEndGameTrigger();
                }
                break;
            default:
                break;
        }
    }

    private void MonsterMove(float speed)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }

    internal Vector3 GetLookAtPoint()
    {
        return lookAtPosition.position;
    }

    public void SetMonsterState(monsterStatesEnm state)
    {
        currentState = state;
    }

    [SerializeField] GameObject BloodEffectCanvas = null;
    float BloodTimer = 0f;
    private void TriggerBloodEffect()
    {
        if (BloodTimer > .25f)
        {
            if (BloodEffectCanvas) BloodEffectCanvas.SetActive(!BloodEffectCanvas.activeSelf);
            BloodTimer = 0f;
            return;
        }
        BloodTimer += Time.deltaTime;
    }
    private void MakePlayerLookAtMonster()
    {
        Debug.Log("LockMenuControl");
        myCamera.SetActive(true);
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        Vector3 LookAtPlayer = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z); //to use the same ground
        transform.LookAt(LookAtPlayer);
        FindObjectOfType<MouseLook>().LockAndLookAtPoint(GetLookAtPoint());
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    private void BadEndGameTrigger()
    {
        Debug.Log("Game has been ended with BAD END! :-)");
        //make something to end the game
        //currently just disables monster
        AudioManager.instance.StopAllSounds();
        //LevelLoader.instance.ending = Ending.Bad;

        StartCoroutine(LevelLoader.instance.StartLoadingSameScene(2f));
        currentState = monsterStatesEnm.end_of_state;
        //StartCoroutine(LevelLoader.instance.StartLoadingSameScene(2f));
    }
}
