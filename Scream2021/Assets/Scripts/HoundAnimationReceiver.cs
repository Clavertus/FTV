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

    // This C# function can be called by an Animation Event
    public void MonsterSound0()
    {
        Debug.Log("MonsterSound0");
        hound.MonsterSound0();
    }
    // This C# function can be called by an Animation Event
    public void MonsterSound1()
    {
        Debug.Log("MonsterSound1");
        hound.MonsterSound1();
    }
    // This C# function can be called by an Animation Event
    public void Scream()
    {
        Debug.Log("Scream");
        hound.Scream();
    }
    // This C# function can be called by an Animation Event
    public void Impact()
    {
        Debug.Log("Impact");
        hound.Impact();
    }
}
