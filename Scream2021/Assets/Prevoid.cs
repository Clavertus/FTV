using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prevoid : MonoBehaviour
{
    public Light[] lights;
    Color tinted;

    public void Awake()
    {
        StartCoroutine("Timer");
        tinted = new Color(0.6650944f, 0.9123682f, 1);
    }

    

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        

       
        for(int i = 0; i < 20; i++)
        {
            Flick();
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            Unflick();
            yield return new WaitForSeconds(Random.Range(0.02f, 0.06f));
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

    public void Unflick()
    {
        foreach (Light a in lights)
        {
            a.type = LightType.Point;
            a.color = tinted;
        }
    }


}
