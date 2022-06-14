using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrailerCinematic : MonoBehaviour
{
    [SerializeField] PlayableDirector cinematic = null;
    [SerializeField] soundsEnum cinematicMusic = soundsEnum.Credits;

    // Start is called before the first frame update
    void Start()
    {
        cinematic.played += CinematicStart;
        cinematic.stopped += CinematicEnd;
        AudioManager.instance.StartPlayingFromAudioManager(cinematicMusic);
        cinematic.Play();
    }

    private void CinematicStart(PlayableDirector obj)
    {
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
    }

    private void CinematicEnd(PlayableDirector obj)
    {
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
        FindObjectOfType<MouseLook>().UnlockFromPoint();
        FindObjectOfType<PlayerMovement>().UnlockPlayer();
    }
}
