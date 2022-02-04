using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaraEpisodeManager : MonoBehaviour
{
    private PlayerMovement player = null;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.SetRunEnable(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
