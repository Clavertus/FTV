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


    private void OnEnable()
    {
        Debug.Log(gameObject.name);  
    }
    void Start()
    {

        selectableCanvas.gameObject.SetActive(false);  

    }

    void Update()
    {
        
    }

    //enables canvas that lets player know the object can be selected
    public void DisplaySelectable()
    {
        Debug.Log(gameObject + " showing selectable");
        selectableCanvas.gameObject.SetActive(true);
    }

    
    public void DisableSelectable()
    {
        selectableCanvas.gameObject.SetActive(false);

    }
    public void DelayedDisableSelectable()
    {
        StartCoroutine(WaitAndDisable());
    }
    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(.2f);
        selectableCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //if player enters this objects selection zone, change its tag to selectable
        if (other.CompareTag("Player"))
        {
            gameObject.tag = ("Selectable");
            AudioManager.instance.PlayFromAudioManager(soundsEnum.UI1);  
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
