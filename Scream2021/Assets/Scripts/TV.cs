using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    public GameObject[] posters;


    void Start()
    {
        StartCoroutine("Scrolling");

    }


    public IEnumerator Scrolling()
    {

        for (int i = 0; i < posters.Length; i++)
        {
            if (i == 0)
            {
                posters[0].SetActive(true);
                yield return new WaitForSeconds(7);
            }
            else
            {
                posters[i].SetActive(true);
                posters[i - 1].SetActive(false);
                yield return new WaitForSeconds(7);
            }
            

        }


    }



}
