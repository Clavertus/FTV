using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundVolumeSlider : MonoBehaviour
{
    public Slider soundSlider;
    public AudioMixer am;


    public void ValueChange()
    {
        float sliderVal = soundSlider.value;
        am.SetFloat("masterVol", 20*Mathf.Log10(sliderVal));
    }

}
