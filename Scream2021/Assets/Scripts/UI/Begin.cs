using FTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Begin : MonoBehaviour {
    public GameObject zoomInPanel;
    public float zoomAmount;
    public float zoomDuration;

    public bool beginning;

    public GameObject settingsCanvas;
    public GameObject quitCanvas;

    [SerializeField] Toggle showFpsToogle = null;
    public Slider sensivitySlider;

    void Start()
    {
        beginning = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        quitCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Title);

        if (PlayerPrefs.HasKey("Setting_ShowFps"))
        {
            bool showFPS = PlayerPrefs.GetInt("Setting_ShowFps") == 1;
            if (showFpsToogle) showFpsToogle.isOn = showFPS;
        }

        if (showFpsToogle) showFpsToogle.onValueChanged.AddListener(delegate { showFpsListener(); });

        if (sensivitySlider) sensivitySlider.onValueChanged.AddListener(delegate { mouseSensivityChanged(); });

        if (PlayerPrefs.HasKey("mouse_sensivity")) sensivitySlider.value = PlayerPrefs.GetFloat("mouse_sensivity");
    }
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !settingsCanvas.activeInHierarchy && !quitCanvas.activeInHierarchy && !beginning)
        {
            PlayButtonSound();
            BeginGame();
        }

        else if (Input.GetKeyDown(KeyCode.Q) && !settingsCanvas.activeSelf && !beginning)
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && !quitCanvas.activeSelf && !beginning)
        {
            ToggleSettingsCanvas();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.Y) && quitCanvas.activeSelf && !beginning)
        {
            QuitGame();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.N) && quitCanvas.activeSelf && !beginning)
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }
    }

    public void BeginGame()
    {
        beginning = true;
        Cursor.visible = false;

        FindObjectOfType<TitleSavingWrapper>().DeleteSaveFile();

        AudioManager.instance.StopFromAudioManager(soundsEnum.Title);

        StartCoroutine(ZoomPanel());
        Debug.Log("Beginning");
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
    public void ToggleSettingsCanvas()
    {
        if (settingsCanvas.activeSelf)
        {
            settingsCanvas.SetActive(false);
        }
        else
        {
            settingsCanvas.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Debug.Log("QuitingGame");
        Application.Quit();
    }

    private void PlayButtonSound()
    {
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIClick);
    }

    private void showFpsListener()
    {
        PlayerPrefs.SetInt("Setting_ShowFps", (showFpsToogle.isOn ? 1 : 0));
    }
    public void mouseSensivityChanged()
    {
        Debug.Log(sensivitySlider.value);
        PlayerPrefs.SetFloat("mouse_sensivity", sensivitySlider.value);
    }

    private IEnumerator ZoomPanel()
    {
        float t = 0;
        float scale = zoomInPanel.GetComponent<RectTransform>().localScale.x;
        while (t < zoomDuration)
        {
            scale = Mathf.Lerp(scale, zoomAmount, t / zoomDuration);
            t += Time.deltaTime;
            zoomInPanel.GetComponent<RectTransform>().transform.localScale = new Vector2(scale, scale);
            yield return null;
        }
        LevelLoader.instance.LoadNextScene();
    } 
}
