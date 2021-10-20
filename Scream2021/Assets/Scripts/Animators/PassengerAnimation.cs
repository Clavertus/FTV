using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerAnimation : MonoBehaviour
{
    [SerializeField] bool isPose = false;
    [Range(0, 10)]
    [SerializeField] int animationId = 0;
    int curranimationId = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (curranimationId != animationId)
        {
            curranimationId = animationId;
            if (isPose)
            {
                selectPose();
            }
            else
            {
                selectAnim();
            }
        }
    }

    private void selectAnim()
    {
        Animator animator = GetComponent<Animator>();

        if (!animator) return;
        switch (animationId)
        {
            case 0:
                animator.SetTrigger("0");
                break;
            case 1:
                animator.SetTrigger("1");
                break;
            case 2:
                animator.SetTrigger("2");
                break;
            case 3:
                animator.SetTrigger("3");
                break;
            case 4:
                animator.SetTrigger("4");
                break;
            case 5:
                animator.SetTrigger("5");
                break;
            case 6:
                animator.SetTrigger("6");
                break;
            case 7:
                animator.SetTrigger("7");
                break;
            case 8:
                animator.SetTrigger("8");
                break;
            case 9:
                animator.SetTrigger("9");
                break;
            case 10:
                animator.SetTrigger("10");
                break;
            default:
                //set to last
                animator.SetTrigger("10");
                break;
        }
    }

    private void selectPose()
    {
        Animator animator = GetComponent<Animator>();

        if (!animator) return;
        switch (animationId)
        {
            case 0:
                animator.SetTrigger("Pose0");
                break;
            case 1:
                animator.SetTrigger("Pose1");
                break;
            case 2:
                animator.SetTrigger("Pose2");
                break;
            case 3:
                animator.SetTrigger("Pose3");
                break;
            case 4:
                animator.SetTrigger("Pose4");
                break;
            case 5:
                animator.SetTrigger("Pose5");
                break;
            default:
                //set to last
                animator.SetTrigger("Pose5");
                break;
        }
    }
}
