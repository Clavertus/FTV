using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundAnimationReceiver : MonoBehaviour
{
    [SerializeField] EndlessTrainMonsterCntrl hound = null;
    // This C# function can be called by an Animation Event
    public void Footstep()
    {
        Debug.Log("FOOTSTEP");
        hound.Footstep();
    }

    // This C# function can be called by an Animation Event
    public void FinishAttack()
    {
        Debug.Log("FINISH ATTACK");
        hound.FinishAttack();
    }
}
