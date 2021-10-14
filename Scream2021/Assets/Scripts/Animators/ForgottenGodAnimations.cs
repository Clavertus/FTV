using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgottenGodAnimations : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    public void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }
    public void PlayOpen()
    {
        animator.SetTrigger("Open");
    }
    public void PlayClose()
    {
        animator.SetTrigger("Close");
    }
}
