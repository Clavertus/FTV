using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideTrainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.instance.PlayFromAudioManager(soundsEnum.Drone);
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
