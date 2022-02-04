using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public enum NpcAnimationState
    {
        idle,
        talk,
        walk,
        sit_down,
        sit,
        sit_and_talk,
        stand_up
    };

    [SerializeField] Animator animator = null;
    private NpcAnimationState currentState = NpcAnimationState.idle;
    private NpcAnimationState lastState = NpcAnimationState.idle;

    // Update is called once per frame
    void Update()
    {
        if(lastState != currentState)
        {
            animator.SetTrigger(((int)currentState).ToString());
            lastState = currentState;
        }
    }

    public void SetAnimation(NpcAnimationState id)
    {
        currentState = id;
    }
}
