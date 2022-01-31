using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainMonsterCntrl : MonoBehaviour
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
        chew
    };

    [SerializeField] Animator myAnimator = null;
    [SerializeField] Transform lookAtPosition = null;
    [SerializeField] Transform playerTransform = null;

    private monsterStatesEnm currentState = monsterStatesEnm.idle;
    private monsterStatesEnm lastState = monsterStatesEnm.idle;
    public AudioSource monsterFootstep;

    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;

    private void OnEnable()
    {
        monsterFootstep = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.HeavyFootstep1);
    }

    // Start is called before the first frame update
    void Start()
    {
        int idle = (int) monsterStatesEnm.idle;
        myAnimator.SetTrigger(((int)idle).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(lastState != currentState)
        {
            myAnimator.SetTrigger(((int)currentState).ToString());
        }

        switch(currentState)
        {
            case monsterStatesEnm.idle:
                break;
            case monsterStatesEnm.run:
                MonsterMove(runSpeed);
                break;
        }

        lastState = currentState;
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

    // This C# function can be called by an Animation Event
    public void Footstep()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterFootstep);
    }
}
