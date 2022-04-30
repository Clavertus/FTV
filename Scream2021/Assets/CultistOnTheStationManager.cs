using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistOnTheStationManager : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] float startupDelay = 4f;
    [SerializeField] float trainSoundDelay = 1f;
    [SerializeField] float delayCultist = 1f;
    [Header("Train Ref")]
    [SerializeField] Transform train = null;
    [SerializeField] float trainSpeed = 5f;
    [SerializeField] Transform lookAtTrain = null;
    [SerializeField] Transform trainPassPosition = null;

    [Header("NPC Ref")]
    [SerializeField] GameObject cultist = null;

    [Header("Music")]
    [SerializeField] soundsEnum soundToPlay = soundsEnum.TaraTalkingBackground;
    [SerializeField] soundsEnum trainSound = soundsEnum.TaraTalkingBackground;

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

        AudioManager.instance.StartPlayingFromAudioManager(trainSound);
        yield return new WaitForSeconds(trainSoundDelay);
        state = scene_state.start_train;
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
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
            StartCoroutine(TriggerCultist());
        }
    }

    private IEnumerator TriggerCultist()
    {
        yield return new WaitForSeconds(delayCultist);
        cultist.SetActive(true);
        playerLook.LockAndLookAtPoint(cultist.GetComponent<NPCLookAtPlayer>().GetLookAtPoint().position); //look at cultist
    }
}
