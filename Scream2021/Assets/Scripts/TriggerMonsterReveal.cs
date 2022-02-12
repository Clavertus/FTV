using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TriggerMonsterReveal : MonoBehaviour
{
    [SerializeField] GameObject monsterToReveal = null;
    [SerializeField] EndlessTrainLevelCntrl levelCntrl = null;
    [SerializeField] PlayableDirector cinematicSequence = null;
    [SerializeField] GameObject DoorToDestroy = null;


    [SerializeField] Transform playerLookCamera = null;
    [SerializeField] Transform playerCinematicCamera = null;
    InGameMenuCotrols menu;
    MouseLook mouseLook;
    PlayerMovement player;

    private void Start()
    {
        menu = FindObjectOfType<InGameMenuCotrols>();
        mouseLook = FindObjectOfType<MouseLook>();
        player = FindObjectOfType<PlayerMovement>();

        player.SetRunEnable(false);
        GetComponent<OpeningDoor>().OnDoorOpened += RevealMonsterStart;
        cinematicSequence.played += LockPlayerControl;
        cinematicSequence.stopped += UnlockPlayerControl;
    }

    public void RevealMonsterStart()
    {
        StartCinematicRevealMonster();
    }

    public void CinematicSlowMotionActivation()
    {
        Time.timeScale = 0.45f;
    }
    public void CinematicShowTutorial()
    {
        FindObjectOfType<DialogueUI>().ShowTutorialBox(0);
    }

    public void CinematicFlickOff()
    {
        levelCntrl.FlickOff();
    }

    public void CinematicMonsterRevealActivation()
    {
        monsterToReveal.SetActive(true);
        monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().SetMonsterState(EndlessTrainMonsterCntrl.monsterStatesEnm.reveal);
        player.SetRunEnable(true);
    }
    public void CinematicMonsterRunActivation()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone2);
        monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().SetMonsterState(EndlessTrainMonsterCntrl.monsterStatesEnm.walk);
    }

    public void CinematicNormalMotionActivation()
    {
        Time.timeScale = 1f;
    }

    public void StartCinematicRevealMonster()
    {
        Debug.Log("StartCinematicRevealMonster");

        levelCntrl.TriggerFlick(0.10f);

        DoorToDestroy.SetActive(false);

        cinematicSequence.Play();
    }

    private void LockPlayerControl(PlayableDirector pd)
    {
        playerCinematicCamera.rotation = playerLookCamera.rotation;
        playerCinematicCamera.position = playerLookCamera.position;

        Debug.Log("LockMenuControl");
        menu.LockMenuControl();
        mouseLook.LockCamera();
        player.LockPlayer();
    }

    private void UnlockPlayerControl(PlayableDirector pd)
    {
        menu.UnlockMenuControl();
        mouseLook.UnlockFromPoint();
    }
}
