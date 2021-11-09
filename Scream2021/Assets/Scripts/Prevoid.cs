using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prevoid : MonoBehaviour
{
    public Light[] lights;
    public GameObject[] figures;
    Color tinted;

    public Transform player;
    public GameObject player2;
    public GameObject cam;
    public GameObject cam2;

    public float camXrot;
    public float playerYrot;


    public int nextSample;
    public int totalSamples;
    public AudioSource track;
    public AudioSource[] componentsArr;

    public int[] blackSamples;
    public int[] whiteSamples;
    public int[] flickSamples;
    public int blackSampleIndex;
    public int whiteSampleIndex;
    public int sampleIndex;

    float accumulatedTime;

    //public static Prevoid instance;




    public void Awake()
    {
        
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        AudioManager.instance.PlayFromAudioManager(soundsEnum.PrevoidTrack);
        accumulatedTime = 0;
        StartCoroutine("Timer");
        tinted = new Color(0.6650944f, 0.9123682f, 1);
        DontDestroyOnLoad(gameObject);
        */
        //TODO: find another place for this function call
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach(TrainEffectController train in trains)
        {
            train.SetPosterMatId(0);
        }
        //DontDestroyOnLoad(gameObject);
        StartSequence();
    }

    private void Start()
    {
        
    }

    public void StartSequence()
    {
        //if(LevelLoader.instance.HasPlayedTheGame)
        { 
            PlayerPrefs.DeleteKey("playerYrot");
            PlayerPrefs.DeleteKey("camXrot");
            AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.PrevoidTrack);
            componentsArr = AudioManager.instance.gameObject.GetComponents<AudioSource>();
            foreach(AudioSource a in componentsArr)
            {
                if(a.clip.name == "Pre-voidTrainAmbience_weird_at_0.42_stop_at_1.04")
                {
                    track = a;
                }
            }

            accumulatedTime = 0;
            Timer();
            tinted = new Color(0.6650944f, 0.9123682f, 1);
        }
    }

   


  



    public void Timer()
    {
        flickSamples = new int[300];
        whiteSamples = new int[blackSamples.Length];
        sampleIndex = 0;
        blackSampleIndex = 0;
        whiteSampleIndex = 0;

        for(int i = 0; i < blackSamples.Length; i++)
        {
            whiteSamples[i] = blackSamples[i] + Random.Range(1470, 4410);  // 0.03 - 0.1 seconds converted into timesamples with frequency = 44100Hz 
        }


    }

    public void TimerEnd()
    {
        playerYrot = player.rotation.eulerAngles.y;
        camXrot = cam.transform.rotation.eulerAngles.x;

        PlayerPrefs.SetFloat("playerYrot", playerYrot);
        PlayerPrefs.SetFloat("camXrot", camXrot);
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }


    public void Blackout()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
            a.intensity = Random.Range(.2f, 1.3f);
        }
    }

    public void Light()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
            a.intensity = 6;
        }
    }





    public void Update()
    {
        
        /*
         *  Use this mechanism to set new flicks. Run the Prevoid scene, press "Z" whenever you want the lights
         *  to flick. Pause just barely the track ends, copy the values from FlickSamples array to
         *  BlackSamples array and save.
         *  Max flicks = 300. Can be modified in Timer().
         *  
         *  
        if (Input.GetKeyDown("z"))
        {
            flickSamples[sampleIndex] = track.timeSamples;
            sampleIndex++;
        }
        */
       

        if(track.timeSamples > blackSamples[blackSampleIndex])
        {
            Blackout();
            blackSampleIndex++;
        }

        if(track.timeSamples > whiteSamples[whiteSampleIndex])
        {
            Light();
            whiteSampleIndex++;
        }  
    }

}
