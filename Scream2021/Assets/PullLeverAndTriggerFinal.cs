using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PullLeverAndTriggerFinal : MonoBehaviour
{
    [SerializeField] Transform leverToPull = null;
    [SerializeField] float leverTargetAngle = -180f;
    [SerializeField] float leverSpeedInSec = -180f;
    [Header("Final Sequence Objects")]
    [SerializeField] GameObject taraMonster = null;
    [SerializeField] GameObject[] finalObjToHide = null;
    [SerializeField] Transform TrainToDisconnect = null;
    [SerializeField] float MaxTrainDissconectSpeed = 25f;
    [SerializeField] float MinTrainDissconectSpeed = 15f;
    [SerializeField] PlayableDirector escapeCinematic = null;

    bool rotateToTarget = false;
    bool reachedTarget = false;
    float currentSpeed = 0f;
    private void Start()
    {
        currentSpeed = MaxTrainDissconectSpeed;
        escapeCinematic.played += LockPlayerControl;
        escapeCinematic.stopped += UnlockPlayerControl;

        rotateToTarget = false;
        if (leverTargetAngle < 0)
        {
            leverTargetAngle = 360 - Mathf.Abs(leverTargetAngle);
        }
    }

    int interactionCounter = 0;
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { Interaction(); return; }

        Rotate();

        if(reachedTarget)
        {
            MoveTrain();
        }
    }

    private void MoveTrain()
    {
        TrainToDisconnect.transform.position = TrainToDisconnect.transform.position + Vector3.forward * currentSpeed * Time.deltaTime;
        if(currentSpeed < 0)
        {
            if (currentSpeed < MinTrainDissconectSpeed)
            {
                currentSpeed += Time.deltaTime;
            }
        }
        else
        {
            if (currentSpeed > MinTrainDissconectSpeed)
            {
                currentSpeed -= Time.deltaTime;
            }
        }
    }

    private void Interaction()
    {
        interactionCounter++;
        rotateToTarget = true;
    }

    private void Rotate()
    {
        if (rotateToTarget)
        {
            Debug.Log(leverTargetAngle);
            Debug.Log(leverToPull.localEulerAngles.y);
            if ((leverToPull.localEulerAngles.z <= leverTargetAngle + 10f) && (leverToPull.localEulerAngles.z >= leverTargetAngle - 10f))
            {
                leverToPull.localEulerAngles = new Vector3(
                    leverToPull.localEulerAngles.x,
                    leverToPull.localEulerAngles.y,
                    leverTargetAngle
                    );
                if (reachedTarget == false)
                {
                    reachedTarget = true;
                    StartCoroutine(FinalSequence());
                }
            }
            else
            {
                leverToPull.localEulerAngles = new Vector3(
                    leverToPull.localEulerAngles.x,
                    leverToPull.localEulerAngles.y,
                    leverToPull.localEulerAngles.z + leverSpeedInSec * Time.deltaTime
                    );

                if ((leverToPull.localEulerAngles.z <= leverTargetAngle + 5f) && (leverToPull.localEulerAngles.z >= leverTargetAngle - 5f))
                {
                    leverToPull.localEulerAngles = new Vector3(
                        leverToPull.localEulerAngles.x,
                        leverToPull.localEulerAngles.y,
                        leverTargetAngle
                        );
                    if(reachedTarget == false)
                    {
                        reachedTarget = true;
                        StartCoroutine(FinalSequence());
                    }
                }
            }
        }
    }

    private IEnumerator FinalSequence()
    {
        escapeCinematic.Play();
        if(taraMonster.activeSelf)
        {
            taraMonster.GetComponent<TaraMonsterController>().SetMonsterState(TaraMonsterController.monsterStatesEnm.idle);
        }
        yield return null;
    }


    //called via signal emmiter
    public void StartLoadingNextScene()
    {
        escapeCinematic.played -= LockPlayerControl;
        escapeCinematic.stopped -= UnlockPlayerControl;

        StartCoroutine( LevelLoader.instance.StartLoadingNextScene() );
    }

    private void LockPlayerControl(PlayableDirector pd)
    {
        Debug.Log("LockMenuControl");
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
    }

    private void UnlockPlayerControl(PlayableDirector pd)
    {
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
        FindObjectOfType<MouseLook>().UnlockFromPoint();
        FindObjectOfType<PlayerMovement>().UnlockPlayer();
    }
}
