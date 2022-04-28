using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistOnTheStationManager : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] float startupDelay = 4f;
    [SerializeField] float trainPassedDelay = 1f;
    [Header("Train Ref")]
    [SerializeField] Transform train = null;
    [SerializeField] float trainSpeed = 5f;
    [SerializeField] Transform lookAtTrain = null;
    [SerializeField] Transform trainPassPosition = null;

    [Header("NPC Ref")]
    [SerializeField] GameObject cultist = null;

    [Header("Music")]
    [SerializeField] soundsEnum soundToPlay = soundsEnum.TaraTalkingBackground;

    PlayerMovement playerMovement = null;
    MouseLook playerLook = null;

    enum scene_state 
    { 
        wait_delay,
        start_train,
        start_cultist
    };
    scene_state state = scene_state.wait_delay;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        cultist.SetActive(false);
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerLook = FindObjectOfType<MouseLook>();

        playerMovement.LockPlayer();

        AudioManager.instance.StartPlayingFromAudioManager(soundToPlay);
        yield return new WaitForSeconds(startupDelay);

        state = scene_state.start_train;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.LockPlayer();
        if (state == scene_state.start_train)
        {
            playerLook.LockAndLookAtPoint(lookAtTrain.position); //look at train
            MoveTrain();
        }
    }

    private void MoveTrain()
    {
        train.position = new Vector3(train.position.x, train.position.y, train.position.z + trainSpeed * Time.deltaTime);
        if(train.position.z >= trainPassPosition.position.z)
        {
            state = scene_state.start_cultist;
            cultist.SetActive(true);
            playerLook.LockAndLookAtPoint(cultist.GetComponent<NPCLookAtPlayer>().GetLookAtPoint().position); //look at cultist
        }
    }
}
