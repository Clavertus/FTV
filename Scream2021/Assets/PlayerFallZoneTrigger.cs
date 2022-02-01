using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerFallZoneTrigger : MonoBehaviour
{
    [SerializeField] GameObject elderGod = null;
    [SerializeField] PlayableDirector cinematicSequence = null;
    [SerializeField] Material elderGodMaterial = null;
    PlayerMovement player = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (player && other.GetComponent<PlayerMovement>())
        {
            //player found
            Debug.Log("Trigger fall zone");
            StartCoroutine(FallToWakeUp());
        }
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        elderGod.SetActive(false);
    }

    bool triggerBloodEffect = false; 
    private void Update()
    {
        if (triggerBloodEffect)
        {
            TriggerBloodEffect();
        }
    }

    private IEnumerator FallToWakeUp()
    {
        player.MakeSimulateFall();
        cinematicSequence.Play();

        elderGod.SetActive(true);

        yield return new WaitForSeconds(.5f);
        triggerBloodEffect = true;

        yield return new WaitForSeconds(2.75f);

        //Transition to another scene -> with TARA
        LevelLoader.instance.ending = Ending.Good;
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }

    [SerializeField] GameObject BloodEffectCanvas = null;
    float BloodTimer = 0f;
    float BloodTimerResetValue = 0.25f;
    float BloodTimerAccelerationValue = 0.01f;
    private void TriggerBloodEffect()
    {
        if (BloodTimer > BloodTimerResetValue)
        {
            if (BloodEffectCanvas) BloodEffectCanvas.SetActive(!BloodEffectCanvas.activeSelf);
            BloodTimer = 0f;
            if(BloodTimerResetValue > BloodTimerAccelerationValue)
            {
                BloodTimerResetValue -= BloodTimerAccelerationValue;
            }
            return;
        }
        BloodTimer += Time.deltaTime;
    }
}
