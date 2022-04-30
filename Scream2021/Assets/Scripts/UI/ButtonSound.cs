using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void HoverSound()
    {
        //AudioManager.instance.InstantStopFromAudioManager(soundsEnum.UI1);
        //AudioManager.instance.InstantPlayOneShotFromAudioManager(soundsEnum.UI1);
    }

    public void ClickSound()
    {
        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.UIClick);
        AudioManager.instance.InstantPlayOneShotFromAudioManager(soundsEnum.UIClick);
    }
}
