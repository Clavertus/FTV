using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCreatureAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    public void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }
    public void PlayWalk()
    {
        animator.SetTrigger("Walk");
    }
    public void PlayRun()
    {
        animator.SetTrigger("Run");
    }
    public void PlayIdleToActive()
    {
        animator.SetTrigger("IdleToActive");
    }
    public void PlayIdleActive()
    {
        animator.SetTrigger("IdleActive");
    }
    public void PlayRunActive()
    {
        animator.SetTrigger("RunActive");
    }
    public void PlayOpenDoorActive()
    {
        animator.SetTrigger("OpenDoorFullActive");
    }
    public void PlayOpenDoorInactive()
    {
        animator.SetTrigger("OpenDoorFullInactive");
    }
    public void PlayRunToJump()
    {
        animator.SetTrigger("RunToJump");
    }
    public void PlayInJump()
    {
        animator.SetTrigger("InJump");
    }
}
