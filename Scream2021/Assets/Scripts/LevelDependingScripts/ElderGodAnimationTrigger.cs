using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderGodAnimationTrigger : MonoBehaviour
{
    [SerializeField] GodPointMovement movement = null;
    [SerializeField] Animator animator = null;
    [SerializeField] bool openOnAwake = false;
    [SerializeField] bool openSlow = true;
    [SerializeField] bool idleOnAwake = true;
    [SerializeField] GameObject explosion = null;
    [SerializeField] GameObject[] objectToDissappear = null;
    [SerializeField] GameObject elderGodObjectToDissapear = null;
    [SerializeField] soundsEnum effectSound = soundsEnum.WhooshEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (idleOnAwake)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            if (openOnAwake)
            {
                if(openSlow)
                {
                    animator.SetTrigger("OpenSlow");
                }
                else
                {
                    animator.SetTrigger("Open");
                }
            }
        }
    }
    public void TriggerAppear()
    {
        StartCoroutine(AnimateAppear());
    }

    //called by cinematic controller (signal emmiter)
    public void TriggerClose()
    {
        StartCoroutine(AnimateDissapear());
    }

    private IEnumerator AnimateAppear()
    {
        AudioManager.instance.StartPlayingFromAudioManager(effectSound);
        if (explosion) explosion.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        elderGodObjectToDissapear.SetActive(true);
        animator.SetTrigger("Idle");
        if (movement) movement.enabled = true;
        yield return new WaitForSeconds(5f);
        if (explosion) explosion.SetActive(false);
    }

    private IEnumerator AnimateDissapear()
    {
        if(movement) movement.enabled = false;
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.15f);
        if (explosion) explosion.SetActive(true);
        AudioManager.instance.StartPlayingFromAudioManager(effectSound);
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject obj in objectToDissappear)
        {
            obj.SetActive(false);
        }

        if(FindObjectOfType<Tara_Behaviour>()) FindObjectOfType<Tara_Behaviour>().elderGodDissapeared = true;
        elderGodObjectToDissapear.SetActive(false);
    }
}
