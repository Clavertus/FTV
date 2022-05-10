using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuCotrols : MonoBehaviour
{
    bool menuActive = true;
    public GameObject settingsPanel;

    void Start()
    {
        menuActive = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (menuActive)
        {
            ProcessInputsFromUser();
        }
    }

    private void ProcessInputsFromUser()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsPanel.activeSelf)
        {
            PlayButtonSound();
            OpenMenu();
        }

        else if (settingsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
        StartCoroutine(LevelLoader.instance.StartLoadingScene(LevelLoader.mainMenuSceneIndex));
    }

    private void PlayButtonSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIClick);
    }

    public void LockMenuControl()
    {
        Debug.Log("LockMenuControl");
        menuActive = false;
    }
    public void UnlockMenuControl()
    {
        menuActive = true;
    }
}
