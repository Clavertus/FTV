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

    // Start is called before the first frame update
    void Start()
    {
        int idle = (int) monsterStatesEnm.idle;
        myAnimator.SetTrigger(idle.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal Vector3 GetLookAtPoint()
    {
        return lookAtPosition.position;
    }
}
