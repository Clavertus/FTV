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
            AudioManager.instance.PlayFromAudioManager(soundsEnum.PrevoidTrack);
            accumulatedTime = 0;
            StartCoroutine("Timer");
            tinted = new Color(0.6650944f, 0.9123682f, 1);
        }
    }

    public float FlickTime(int a = 1)
    {
        if (a == 0)
        {
            float b = Random.Range(0.05f, 0.15f);
            accumulatedTime += b;

            return b;                           //long
        }
        else
        {

            float b = Random.Range(0.02f, 0.1f);
            accumulatedTime += b;

            return b;
            //short
        }

    }

    public IEnumerator Timer()
    {
        //DURATION OF THE SOUNDTRACK - 1:07

        #region flickingSequence

        /* This sequence is hard-coded to get a good non-repeatable expierience.
         * It's done on purpose, lol*/


        yield return new WaitForSecondsRealtime(5f);


        //single flick 10 seconds in
        LowFlick(1);
        yield return new WaitForSecondsRealtime(FlickTime());
        LowUnFlick();







        yield return new WaitForSecondsRealtime(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(2f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }



        yield return new WaitForSecondsRealtime(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(2f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }







        yield return new WaitForSecondsRealtime(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(1));
        }




        yield return new WaitForSecondsRealtime(5f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 5; i++)
        {
            LowFlick(2);
            yield return new WaitForSecondsRealtime(FlickTime());
            LowUnFlick();
            yield return new WaitForSecondsRealtime(FlickTime(2));
        }





        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSecondsRealtime(1f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(2);
                yield return new WaitForSecondsRealtime(FlickTime());
                LowUnFlick();
                yield return new WaitForSecondsRealtime(FlickTime(2));
            }
        }







        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSecondsRealtime(.5f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(4);
                yield return new WaitForSecondsRealtime(FlickTime());
                LowUnFlick();
                yield return new WaitForSecondsRealtime(FlickTime(2));
            }
        }



        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSecondsRealtime(.3f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(4);
                yield return new WaitForSecondsRealtime(FlickTime());

                LowUnFlick();
                yield return new WaitForSecondsRealtime(FlickTime(2));
            }
        }


        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSecondsRealtime(1f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                Flick();
                yield return new WaitForSecondsRealtime(FlickTime());

                UnFlick();
                if (j == 1)
                // mannequinnes stand up
                {
                    for (int k = 0; k < figures.Length; k++)
                    {
                        figures[k].GetComponent<PassengerAnimation>().animationId = 4;
                        figures[k].transform.position += figures[k].transform.forward;
                    }
                }

                if (j == 2)
                {

                    for (int k = 0; k < figures.Length; k++)
                    {
                        figures[k].GetComponent<Transform>().LookAt(player);
                    }

                }
                yield return new WaitForSecondsRealtime(FlickTime(2));
            }
        }










        #endregion

        playerYrot = player.rotation.eulerAngles.y;
        camXrot = cam.transform.rotation.eulerAngles.x;
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }


    public void LowFlick(int flickIntensity = 0)
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
            switch (flickIntensity)
            {

                case 0:
                    a.intensity = Random.Range(3f, 4f);
                    break;

                case 1:
                    a.intensity = Random.Range(2f, 4f);
                    break;

                case 2:
                    a.intensity = Random.Range(1f, 3f);
                    break;

                case 3:
                    a.intensity = Random.Range(.5f, 2f);
                    break;

                case 4:
                    a.intensity = Random.Range(.1f, 1f);
                    break;


            }
        }
    }

    public void LowUnFlick()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
            a.intensity = 6f;
        }
    }






    public void Flick()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Directional;
            a.color = Color.red;
        }
    }

    public void UnFlick()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = Color.black;
        }
    }


}
