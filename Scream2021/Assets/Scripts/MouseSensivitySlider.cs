using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensivitySlider : MonoBehaviour
{
	public Slider sensivitySlider;

	MouseLook mouseLook;

    private void Awake()
	{
		mouseLook = FindObjectOfType<MouseLook>();
		if (mouseLook)
		{
			Debug.Log("mouse Look found!");
			if (PlayerPrefs.HasKey("mouse_sensivity")) mouseLook.mouseSensitivity = PlayerPrefs.GetFloat("mouse_sensivity");
		}
		else
		{
			//Debug.LogError("Mouse Look component requirent somewhere on scene to apply mouse sensivity");
		}
	}

    public void Start()
	{
		if(sensivitySlider) sensivitySlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

		if (PlayerPrefs.HasKey("mouse_sensivity")) sensivitySlider.value = PlayerPrefs.GetFloat("mouse_sensivity");

		mouseLook = FindObjectOfType<MouseLook>();
		if(mouseLook)
		{
			if (PlayerPrefs.HasKey("mouse_sensivity")) mouseLook.mouseSensitivity = PlayerPrefs.GetFloat("mouse_sensivity");
		}
		else
		{
			//Debug.LogError("Mouse Look component requirent somewhere on scene to apply mouse sensivity");
		}
	}

	public void ValueChangeCheck()
	{
		Debug.Log(sensivitySlider.value);
		if(mouseLook) mouseLook.mouseSensitivity = sensivitySlider.value;
		PlayerPrefs.SetFloat("mouse_sensivity", sensivitySlider.value);
	}
}
