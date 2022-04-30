using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selectable : MonoBehaviour
{
    
    //the canvas that lets the player know the object can be selected
    [SerializeField] Canvas selectableCanvas;
    [SerializeField] float maxDistanceFromPlayer = 1;
    [SerializeField] bool checkPlayerRotation = false;

    [SerializeField] float minYLookRotation;
    [SerializeField] float maxYLookRotation;

    bool superSecretEnterSelectable = false; 
    bool enteredSelectable = false; 
    AudioSource myAudioSource;
    GameObject player; 

    private void OnEnable()
    {
        //Debug.Log(gameObject.name);  
    }
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        selectableCanvas.gameObject.SetActive(false);  
    }

    private void OnDisable()
    {
        selectableCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        //Debug.Log(this.gameObject.name + this.tag);
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if ((distance <= maxDistanceFromPlayer) && (enteredSelectable == false) && (checkPlayerRotation == false)) { EnterSelectionZone();  }

        if (superSecretEnterSelectable == true && (enteredSelectable == false)) { EnterSelectionZone(); }

        if ((distance <= maxDistanceFromPlayer) && (enteredSelectable == false) && (checkPlayerRotation == true)) 
        {
            Debug.Log(player.transform.eulerAngles.y);
            if (player.transform.eulerAngles.y >= minYLookRotation && player.transform.eulerAngles.y <= maxYLookRotation) 
            EnterSelectionZone(); 
        }

        if ((distance >= maxDistanceFromPlayer) && (enteredSelectable == true) && (superSecretEnterSelectable == false)) { ExitSelectionZone(); }

    }

    //enables canvas that lets player know the object can be selected
    public void DisplaySelectable()
    {
        //Debug.Log(gameObject + " showing selectable");
        selectableCanvas.gameObject.SetActive(true);
    }

    public void BypassAndMakeSelectable() 
    {
        superSecretEnterSelectable = true; 
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

    public void EnterSelectionZone() 
    {
        enteredSelectable = true; 
        gameObject.tag = ("Selectable"); 
        // AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.UI1);  
    }
    public void ExitSelectionZone() 
    {
        superSecretEnterSelectable = false; 
        enteredSelectable = false;  
        gameObject.tag = ("Untagged");
        selectableCanvas.gameObject.SetActive(false);
    }

    public void ChangeUi(Canvas newCanvas)
    {
        selectableCanvas.gameObject.SetActive(false);
        selectableCanvas = newCanvas;
    }
}
