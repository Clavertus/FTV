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
        StartCoroutine(RevealMonster());
    }


    public IEnumerator RevealMonster()
    {
        Debug.Log("RevealMonster");

        levelCntrl.TriggerFlick(0.1f);

        yield return new WaitForSeconds(0.1f);

        cinematicSequence.Play();

        DoorToDestroy.SetActive(false);

        yield return new WaitForSeconds(5.5f);

        levelCntrl.FlickOff();

        monsterToReveal.SetActive(true);

        monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().SetMonsterState(EndlessTrainMonsterCntrl.monsterStatesEnm.reveal);

        FindObjectOfType<DialogueUI>().ShowTutorialBox(0);

        player.SetRunEnable(true);

        yield return new WaitForSeconds(3.5f);

        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone2);

        monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().SetMonsterState(EndlessTrainMonsterCntrl.monsterStatesEnm.walk);
    }

    private void LockPlayerControl(PlayableDirector pd)
    {
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
