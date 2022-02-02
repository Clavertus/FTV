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

    // Start is called before the first frame update
    void Start()
    {

    }

    private NpcAnimationState lastState = NpcAnimationState.idle;
    // Update is called once per frame
    void Update()
    {

    }
}
