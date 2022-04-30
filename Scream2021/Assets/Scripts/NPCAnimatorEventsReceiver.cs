using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimatorEventsReceiver : MonoBehaviour
{
    public Action EventNPCFootStep { get; set; }
    public Action EventNPCSitDownFinished { get; set; }
    public Action EventNPCStandUpFinished { get; set; }

    #region ANIMATION EVENTS
    //animation events
    public void EventAnimatorFootStep ()
    {
        EventNPCFootStep?.Invoke();
    }
    public void EventAnimatorSitDownFinished()
    {
        EventNPCSitDownFinished?.Invoke();
    }
    public void EventAnimatorStandUpFinished()
    {
        EventNPCStandUpFinished?.Invoke();
    }
    #endregion
}
