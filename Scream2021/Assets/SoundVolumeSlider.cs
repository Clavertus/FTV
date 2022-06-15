using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundVolumeSlider : MonoBehaviour
{
    public Slider soundSlider;
    public AudioMixer am;

     public float defaultVol = 0.3f;


    void Start()
    {
        if (PlayerPrefs.HasKey("masterVol"))
        {
            soundSlider.value = PlayerPrefs.GetFloat("masterVol");
            ValueChange();
        } else
        {           
            PlayerPrefs.SetFloat("masterVol", defaultVol);
            soundSlider.value = defaultVol;
            ValueChange();
        }
    }



    public void ValueChange()
    {
        
        float sliderVal = soundSlider.value;
        am.SetFloat("masterVol", 20*Mathf.Log10(sliderVal));
        PlayerPrefs.SetFloat("masterVol", sliderVal);

    }

}
