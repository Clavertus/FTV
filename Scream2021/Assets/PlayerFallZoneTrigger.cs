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
        cinematicSequence.stopped += TransitionToNextScene;
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
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        FindObjectOfType<EndlessTrainMonsterCntrl>().enabled = false;

        //player.MakeSimulateFall();
        cinematicSequence.Play();

        elderGod.SetActive(true);
        triggerBloodEffect = true;

        yield return new WaitForSeconds(.5f);
    }

    private void TransitionToNextScene(PlayableDirector pd)
    {

        AudioManager.instance.StopAllSounds();
        //Transition to another scene -> with TARA
        //LevelLoader.instance.ending = Ending.Good;
        StartCoroutine(LevelLoader.instance.StartLoadingNextSceneWithHardCut());
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
