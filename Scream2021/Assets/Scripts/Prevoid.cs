using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int[] shakeSamples;
    public int blackSampleIndex;
    public int whiteSampleIndex;
    public int sampleIndex;
    public int shakeSampleIndex;

    float accumulatedTime;

    public bool flickering;

    //public static Prevoid instance;

    public void Awake()
    {
        flickering = true;
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        shakeSamples = new int[10];

        sampleIndex = 0;
        blackSampleIndex = 0;
        whiteSampleIndex = 0;
        shakeSampleIndex = 0;

        for (int i = 0; i < blackSamples.Length; i++)
        {
            whiteSamples[i] = blackSamples[i] + Random.Range(1470, 4410);  
            // 0.03 - 0.1 seconds converted into timesamples with frequency = 44100Hz 
        }


        for (int i = 0; i < shakeSamples.Length; i++)
        {
            shakeSamples[i] = Mathf.RoundToInt(blackSamples[blackSamples.Length - 1] / 9) * i;
           
        }




    }

    public void TimerEnd()
    {
        playerYrot = player.rotation.eulerAngles.y;
        camXrot = cam.transform.rotation.eulerAngles.x;

        PlayerPrefs.SetFloat("playerYrot", playerYrot);
        PlayerPrefs.SetFloat("camXrot", camXrot);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
 
        // StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }


    public void Blackout(int type)
    {
        //0 - white
        //1 - red

        if (type == 0)
        {
            foreach (Light a in lights)
            {
                a.type = LightType.Point;
                a.color = tinted;
                a.intensity = Random.Range(0.2f, 4f);
            }

        }
        else
        {

            foreach (Light a in lights)
            {
                a.type = LightType.Directional;
                a.color = Color.black;
                a.intensity = Random.Range(0.2f, 2f);
            }


        }





    }

    public void Light(int type)
    {
        
        if (type == 0)
        {
            foreach (Light a in lights)
            {
                a.type = LightType.Point;
                a.color = tinted;
                a.intensity = 6f;
            }

        }
        else
        {

            foreach (Light a in lights)
            {
                a.type = LightType.Directional;
                a.color = Color.red;
                a.intensity = 6f;
            }


        }
    }





    public void Update()
    {

        /*
       if (Input.GetKeyDown("z"))
       {
           flickSamples[sampleIndex] = track.timeSamples;
           sampleIndex++;
       }
       */

        if (flickering)
        {
            if (track.timeSamples > blackSamples[blackSampleIndex])
            {
                if(track.timeSamples > blackSamples[blackSamples.Length - 10])
                {
                    Blackout(1);
                } else
                {
                    Blackout(0);
                }
                    
                blackSampleIndex++;
            }

            if (track.timeSamples > whiteSamples[whiteSampleIndex])
            {
                if (track.timeSamples > whiteSamples[whiteSamples.Length - 5])
                {
                    Light(1);
                }
                else
                {
                    Light(0);
                }
                whiteSampleIndex++;
                if (whiteSampleIndex == whiteSamples.Length - 3)
                {
                    Blackout(1);
                    flickering = false;
                    TimerEnd();

                };
            }



            if (track.timeSamples > shakeSamples[shakeSampleIndex])
            {            
                shakeSampleIndex++;

                if(shakeSampleIndex > 5)
                cam.GetComponent<CameraShaker>().power += 0.015f;

            }
        }
    }

}
