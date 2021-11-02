using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSkip : MonoBehaviour
{
    [SerializeField] GameObject Skip;

    private void Start()
    {
        Skip.SetActive(false);
    }

    float skipTimerCnt = 0f;
    float skipTimerAppear = 1.5f;
    // Update is called once per frame
    void Update()
    {
        skipTimerCnt += Time.deltaTime;
        if (skipTimerAppear <= skipTimerCnt)
        {
            Skip.SetActive(true);
        }

        if(Skip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                StartCoroutine(LevelLoader.instance.StartLoadingScene(0));
            }
        }
    }
}
