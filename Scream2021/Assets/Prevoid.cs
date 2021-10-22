using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prevoid : MonoBehaviour
{
    public Light[] lights;
    Color tinted;

    float accumulatedTime;


    public void Awake()
    {

        accumulatedTime = 0;     
        StartCoroutine("Timer");
        tinted = new Color(0.6650944f, 0.9123682f, 1);

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


        yield return new WaitForSeconds(5f);


        //single flick 10 seconds in
        LowFlick(1);
        yield return new WaitForSeconds(FlickTime());
        LowUnFlick();







        yield return new WaitForSeconds(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(2f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }



        yield return new WaitForSeconds(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(2f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(4f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }







        yield return new WaitForSeconds(3f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 2; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(1));
        }




        yield return new WaitForSeconds(5f - accumulatedTime);
        accumulatedTime = 0;


        for (int i = 0; i < 5; i++)
        {
            LowFlick(2);
            yield return new WaitForSeconds(FlickTime());
            LowUnFlick();
            yield return new WaitForSeconds(FlickTime(2));
        }





        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(2);
                yield return new WaitForSeconds(FlickTime());
                LowUnFlick();
                yield return new WaitForSeconds(FlickTime(2));
            }
        }


      
        



        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(.5f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(4);
                yield return new WaitForSeconds(FlickTime());
                LowUnFlick();
                yield return new WaitForSeconds(FlickTime(2));
            }
        }



        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(.3f - accumulatedTime);
            accumulatedTime = 0;


            for (int j = 0; j < 3; j++)
            {
                LowFlick(4);
                yield return new WaitForSeconds(FlickTime());
                LowUnFlick();
                yield return new WaitForSeconds(FlickTime(2));
            }
        }

        







        #endregion





    }


    public void LowFlick(int flickIntensity = 0)
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
            switch (flickIntensity) {

                case 0:  a.intensity = Random.Range(3f, 4f);
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
            a.color = tinted;
        }
    }


}
