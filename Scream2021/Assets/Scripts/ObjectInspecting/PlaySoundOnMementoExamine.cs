using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnMementoExamine : MonoBehaviour
{
    [SerializeField] MementoObjectInspectingLookAtPart scriptRef = null;
    [SerializeField] soundsEnum selectedSound = soundsEnum.IntroDrone;

    Transform player = null;
    AudioSource source = null;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        source = AudioManager.instance.AddAudioSourceWithSound(this.gameObject, selectedSound);
        source.volume = 0.25f;
        AudioManager.instance.StartPlayingFromGameObject(source);
    }

    // Update is called once per frame
    public void SetFullSound()
    {
        source.Pause();
        source.volume = 1.0f;
        source.UnPause();
    }
    // Update is called once per frame
    public void SetSilent()
    {
        AudioManager.instance.StopFromGameObject(source);
        AudioManager.instance.InstantStopFromGameObject(source);
    }
}
