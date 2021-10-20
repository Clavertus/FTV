using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selectable : MonoBehaviour
{
    
    //the canvas that lets the player know the object can be selected
    [SerializeField] Canvas selectableCanvas;
    AudioSource myAudioSource;
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        myAudioSource = FindObjectOfType<AudioManager>().AddAudioSourceWithSound(gameObject, soundsEnum.UI1);
    }

    void Start()
    {
        selectableCanvas.enabled = false;  
    }

    void Update()
    {
        
    }

    //enables canvas that lets player know the object can be selected
    public void DisplaySelectable()
    {
       
        selectableCanvas.enabled = true; 
    }

    
    public void DisableSelectable()
    {
        
        selectableCanvas.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManager>().PlayFromGameObject(myAudioSource);
        //if player enters this objects selection zone, change its tag to selectable
        if (other.CompareTag("Player"))
        {
            gameObject.tag = ("Selectable"); 
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if player exits this objects selection zone, change its tag to untagged
        if (other.CompareTag("Player"))
        {
            gameObject.tag = ("Untagged");  
            
        }
    }


}
