using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Begin : MonoBehaviour {
    public GameObject zoomInPanel;
    public float zoomAmount;
    public float zoomSpeed;

    public GameObject settingsCanvas;
    public GameObject quitCanvas;

    [SerializeField] Toggle showFpsToogle = null;
    public Slider sensivitySlider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Setting_ShowFps"))
        {
            bool showFPS = PlayerPrefs.GetInt("Setting_ShowFps") == 1 ? true : false;
            if (showFpsToogle) showFpsToogle.isOn = showFPS;
        }

        if (showFpsToogle) showFpsToogle.onValueChanged.AddListener(delegate { showFpsListener(); });

        if (sensivitySlider) sensivitySlider.onValueChanged.AddListener(delegate { mouseSensivityChanged(); });

        if (PlayerPrefs.HasKey("mouse_sensivity")) sensivitySlider.value = PlayerPrefs.GetFloat("mouse_sensivity");
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        quitCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.TV);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.Drone);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !settingsCanvas.activeSelf && !quitCanvas.activeSelf)
        {
            BeginGame();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.Q) && !settingsCanvas.activeSelf)
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && !quitCanvas.activeSelf)
        {
            ToggleSettingsCanvas();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.Y) && quitCanvas.activeSelf)
        {
            QuitGame();
            PlayButtonSound();
        }

        else if (Input.GetKeyDown(KeyCode.N) && quitCanvas.activeSelf)
        {
            ToggleQuitCanvas();
            PlayButtonSound();
        }
    }

    public void BeginGame()
    {
        AudioManager.instance.StopFromAudioManager(soundsEnum.Drone);
        AudioManager.instance.StopFromAudioManager(soundsEnum.TV);

        StartCoroutine(ZoomPanel());
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
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.UIMetal);
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
        while (zoomAmount != System.Math.Round(zoomInPanel.GetComponent<RectTransform>().transform.localScale.x))
        {
            scale = Mathf.Lerp(scale, zoomAmount, t);
            t += zoomSpeed * Time.deltaTime;
            zoomInPanel.GetComponent<RectTransform>().transform.localScale = new Vector2(scale, scale);
            Debug.Log(zoomInPanel.GetComponent<RectTransform>().transform.localScale.x);
            yield return new WaitForSecondsRealtime(0);
        }
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    } 
}
