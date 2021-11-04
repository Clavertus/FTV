using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCorrection : MonoBehaviour
{

    private GameObject player;
    private GameObject cam;

    private float playerYrot;
    private float camXrot;

    void Awake()
    {
        if(PlayerPrefs.HasKey("playerYrot")) playerYrot = PlayerPrefs.GetFloat("playerYrot");
        if (PlayerPrefs.HasKey("camXrot")) camXrot = PlayerPrefs.GetFloat("camXrot");

        if (PlayerPrefs.HasKey("playerYrot") && PlayerPrefs.HasKey("camXrot"))
        {
            player = GameObject.Find("Player");
            player.transform.eulerAngles = new Vector3(0, playerYrot, 0);
            cam = player.transform.GetChild(0).gameObject;
            cam.transform.eulerAngles = new Vector3(camXrot, 0, 0);
        }
        else
        {
            Debug.Log("Correction could not happen, as data is missing");
        }
    }

}
