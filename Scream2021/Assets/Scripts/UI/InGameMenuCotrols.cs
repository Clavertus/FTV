using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuCotrols : MonoBehaviour
{
    bool menuActive = false;
    public GameObject settingsPanel;

    void Start()
    {
        menuActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsPanel.activeSelf)
        {
            PlayButtonSound();
            OpenMenu();
        }

        else if (settingsPanel.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PlayButtonSound();
                CloseMenu();
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                PlayButtonSound();
                QuitToMainMenu();
            }
        }
    }

    public void CloseMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
    }

    public void OpenMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        foreach (var sound in AudioManager.instance.sounds)
        {
            sound.source.Stop();
        }
        StartCoroutine(LevelLoader.instance.StartLoadingScene(0));
    }

    private void PlayButtonSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIClick);
    }
}
