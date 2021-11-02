using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensivitySlider : MonoBehaviour
{
	public Slider sensivitySlider;

	MouseLook mouseLook;

	public void Start()
	{
		if(sensivitySlider) sensivitySlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

		mouseLook = FindObjectOfType<MouseLook>();
		if(mouseLook)
		{
			Debug.Log("Found!");
			if (mouseLook) mouseLook.mouseSensitivity = sensivitySlider.value;
		}
	}

	public void ValueChangeCheck()
	{
		Debug.Log(sensivitySlider.value);
		if(mouseLook) mouseLook.mouseSensitivity = sensivitySlider.value;
	}
}
