using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void HoverSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UI1);
    }

    public void ClickSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIMetal);
    }
}
