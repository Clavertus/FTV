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

    public GameObject loadButton;
    public GameObject settingsCanvas;
    public GameObject quitCanvas;
    public GameObject newGameCanvas;

    [SerializeField] Toggle showFpsToogle = null;
    public Slider sensivitySlider;

    void Start()
    {
        beginning = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        quitCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        newGameCanvas.SetActive(false);

        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Title);

        if (PlayerPrefs.HasKey("Setting_ShowFps"))
        {
            bool showFPS = PlayerPrefs.GetInt("Setting_ShowFps") == 1;
            if (showFpsToogle) showFpsToogle.isOn = showFPS;
        }

        if (showFpsToogle) showFpsToogle.onValueChanged.AddListener(delegate { showFpsListener(); });

        if (sensivitySlider) sensivitySlider.onValueChanged.AddListener(delegate { mouseSensivityChanged(); });

        if (PlayerPrefs.HasKey("mouse_sensivity")) sensivitySlider.value = PlayerPrefs.GetFloat("mouse_sensivity");

        if(FindObjectOfType<TitleSavingWrapper>().CheckSaveGame() == false)
        {
            loadButton.SetActive(false);
        }
    }
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !settingsCanvas.activeInHierarchy && !quitCanvas.activeInHierarchy && !newGameCanvas.activeInHierarchy && !beginning)
        {
            PlayButtonSound();
            LoadGame();
        }

        else if (Input.GetKeyDown(KeyCode.E) && !settingsCanvas.activeInHierarchy && !quitCanvas.activeInHierarchy && !newGameCanvas.activeInHierarchy && !beginning)
        {
            PlayButtonSound();
            BeginGame();
        }

        else if (Input.GetKeyDown(KeyCode.Q) && !settingsCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            ToggleQuitCanvas();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && !quitCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            ToggleSettingsCanvas();
        }

        else if (Input.GetKeyDown(KeyCode.Y) && quitCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            QuitGame();
        }

        else if (Input.GetKeyDown(KeyCode.N) && quitCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            ToggleQuitCanvas();
        }

        else if (Input.GetKeyDown(KeyCode.Y) && newGameCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            StartNewGame();
        }

        else if (Input.GetKeyDown(KeyCode.N) && newGameCanvas.activeSelf && !beginning)
        {
            PlayButtonSound();
            ToggleNewGameCanvas();
        }
    }

    public void BeginGame()
    {
        if (FindObjectOfType<TitleSavingWrapper>().CheckSaveGame())
        {
            Debug.Log("Save file exists");
            ToggleNewGameCanvas();
        }
        else
        {
            Debug.Log("No Save file exists");
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        beginning = true;
        Cursor.visible = false;

        FindObjectOfType<TitleSavingWrapper>().DeleteSaveFile();

        AudioManager.instance.StopFromAudioManager(soundsEnum.Title);

        StartCoroutine(ZoomPanel(true));

        Debug.Log("Beginning");
    }

    public void LoadGame()
    {
        beginning = true;
        Cursor.visible = false;

        AudioManager.instance.StopFromAudioManager(soundsEnum.Title);

        StartCoroutine(ZoomPanel(false));
        Debug.Log("Loading");
    }

    public void ToggleNewGameCanvas()
    {
        if (newGameCanvas.activeSelf)
        {
            newGameCanvas.SetActive(false);
        }
        else
        {
            newGameCanvas.SetActive(true);
        }
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

    private IEnumerator ZoomPanel(bool IfNewGame)
    {
        if (newGameCanvas.activeSelf)
        {
            newGameCanvas.SetActive(false);
        }

        float t = 0;
        float scale = zoomInPanel.GetComponent<RectTransform>().localScale.x;
        while (t < zoomDuration)
        {
            scale = Mathf.Lerp(scale, zoomAmount, t / zoomDuration);
            t += Time.deltaTime;
            zoomInPanel.GetComponent<RectTransform>().transform.localScale = new Vector2(scale, scale);
            yield return null;
        }

        if (IfNewGame)
        {
            LevelLoader.instance.LoadNextScene();
        }
        else
        {
            StartCoroutine(LevelLoader.instance.StartLoadingSceneFromTitleScreen(2.5f));
        }
    } 
}
