using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour, ISaveable
{
    [SerializeField] NPCAnimatorEventsReceiver eventReceiver = null;
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

    private void Start()
    {
        eventReceiver.EventNPCFootStep += OnFootstepEvent;
        eventReceiver.EventNPCSitDownFinished += OnSitDownEvent;
        eventReceiver.EventNPCStandUpFinished += OnStandUpEvent;
    }

    private void OnStandUpEvent()
    {
        currentState = NpcAnimationState.idle;
    }

    private void OnSitDownEvent()
    {
        currentState = NpcAnimationState.sit;
    }

    private void OnFootstepEvent()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Footstep3);
    }

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

    public NpcAnimationState GetCurrentState()
    {
        return currentState;
    }


    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public int currentState;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.currentState = (int) currentState;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        currentState = (NpcAnimationState) data.currentState;
    }
    #endregion
}