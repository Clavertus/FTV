using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnInspection : MonoBehaviour
{
    [SerializeField] bool playSoundOnInspection = false;
    [SerializeField] soundsEnum sound = soundsEnum.ApplyDPad;

    public int interactionCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playSoundOnInspection == false) return;

        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }
    }

    private void Interaction()
    {
        interactionCounter++;
        AudioManager.instance.PlayOneShotFromAudioManager(sound);
    }
}
