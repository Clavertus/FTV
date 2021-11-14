using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour {
    public GameObject settingsCanvas;
    public GameObject quitCanvas;

    void Start()
    {
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TV);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BeginGame();
            PlayButtonSound();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }

        if (Input.GetKeyDown(KeyCode.Y) && quitCanvas.activeSelf)
        {
            QuitGame();
            PlayButtonSound();
        }

        if (Input.GetKeyDown(KeyCode.N) && quitCanvas.activeSelf)
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }
    }

    public void BeginGame()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.StopFromAudioManager(soundsEnum.TV);

        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }

    public void ToggleQuitCanvas()
    {
        if (quitCanvas.activeSelf)
        {
            quitCanvas.SetActive(false);
        }
        else
        {
            quitCanvas.SetActive(true);
        }
    }
    
    public void QuitGame()
    {
        Debug.Log("QuitingGame");
        Application.Quit();
    }

    private void PlayButtonSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIMetal);
    }
}
