using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCorrection : MonoBehaviour
{

    public GameObject player2;
    public GameObject cam2;
    public Prevoid preVoidManager;

    public float playerYrot;
    public float camXrot;

    void Awake()
    {
        /*
        preVoidManager = Prevoid.instance;
        Debug.Log(preVoidManager);
        playerYrot = preVoidManager.playerYrot;
        camXrot = preVoidManager.camXrot;

        player2 = GameObject.Find("Player");
        player2.transform.eulerAngles = new Vector3(0, playerYrot, 0);
        cam2 = player2.transform.GetChild(0).gameObject;
        cam2.transform.eulerAngles = new Vector3(camXrot, 0, 0);

        Destroy(preVoidManager);
        Destroy(gameObject);
        */
    }


   
}
