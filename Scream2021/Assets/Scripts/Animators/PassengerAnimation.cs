using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PassengerAnimation : MonoBehaviour
{   
    [SerializeField] bool isPose;
    [Range(0, 9)]
    [SerializeField] int animationId = 0;
    [Range(0f, 100000f)]
    [SerializeField] int animationFrame = 0;
    public AnimationClip[] animationClips;
    public AnimationClip[] animationPoses;
    int currentAnimId = 0;
    
    [HideInInspector]
    public bool isInEditorMode = false;
    private Animator animator;

    public void OnSceneLoaded()
    {
        AnimationMode.StopAnimationMode();
        AnimationMode.EndSampling();
        animationClips = null;
        animationPoses = null;
        animationFrame = 0;
        foreach (var item in FindObjectsOfType<PassengerAnimation>())
        {
            item.isInEditorMode = false;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
      
        foreach (var item in FindObjectsOfType<PassengerAnimation>())
        {
            item.isInEditorMode = false;
        }
    }

    void Update()
    {
        if (currentAnimId != animationId && !isInEditorMode)
        {
            currentAnimId = animationId;
            if (isPose)
            {
                SelectPose();
            }
            else
            {
                SelectAnim();
            }
        }
        if (isInEditorMode)
        {
          
            if (isPose)
            {
                SelectPose();
            }
            else
            {
                SelectAnim();
            }
        }
    }

    private void SelectAnim()
    {
        if (!animator) return;
        if (isInEditorMode)
            AnimationMode.SampleAnimationClip(gameObject, animationClips[animationId], animationClips[animationId].length / 100000f * animationFrame);
        else
            animator.SetTrigger(animationId.ToString());
    }

    private void SelectPose()
    {

        if (!animator) return;

        if (animationId <= 5)
        {
            if (isInEditorMode)
                AnimationMode.SampleAnimationClip(gameObject, animationPoses[animationId], animationClips[animationId].length / 100000f * animationFrame);
            else
                animator.SetTrigger("Pose" + animationId);
        }
        else
        {
            if (isInEditorMode)
                AnimationMode.SampleAnimationClip(gameObject, animationPoses[5], animationClips[animationId].length / 100000f * animationFrame);
            else
                animator.SetTrigger("Pose" + 5);
        }
    }

}