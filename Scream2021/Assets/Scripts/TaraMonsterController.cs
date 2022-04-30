using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TaraMonsterController : MonoBehaviour
{
    [SerializeField] PlayableDirector transformationFinishedCinematic = null;
    bool transformationFinished = false;
    public enum monsterStatesEnm
    {
        transform_idle,
        transform_finish,
        idle,
        walk,
        kill,
        jump,
        in_jump,
        impact_and_fall,
        falling,
        end_of_state
    };

    [SerializeField] Animator myAnimator = null;
    [SerializeField] Transform lookAtPosition = null;
    [SerializeField] Transform playerTransform = null;
    [SerializeField] Transform endPosition = null;

    private monsterStatesEnm currentState = monsterStatesEnm.transform_idle;
    private monsterStatesEnm lastState = monsterStatesEnm.transform_idle;

    private AudioSource monsterFootstep;
    private AudioSource monsterSound0;
    private AudioSource monsterAttack0;
    private AudioSource monsterAttack1;
    private AudioSource monsterSound1;
    private AudioSource monsterScream;
    private AudioSource monsterImpact;

    [Header("Sounds")]
    [SerializeField] soundsEnum transformSFX = soundsEnum.TaraSpeech1;
    [SerializeField] soundsEnum transformBackground = soundsEnum.TaraSpeech1;
    [Header("Settings")]
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float minimalDistanceToPlayer = 5f;

    public void MonsterSound0()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterSound0);
    }

    public void MonsterSound1()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterSound1);
    }

    public void Impact()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterImpact);
    }
    public void Attack0()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterAttack0);
    }
    public void Attack1()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterAttack1);
    }

    public void Scream()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterScream);
    }

    public void Footstep()
    {
        AudioManager.instance.InstantPlayFromGameObject(monsterFootstep);
    }

    int finishedChew = 0;
    public void FinishedChew()
    {
        finishedChew += 1;
    }

    private void OnDisable()
    {
        //AudioManager.instance.StopFromAudioManager(transformSFX);
        if(AudioManager.instance) AudioManager.instance.StopFromAudioManager(transformBackground);
    }

    private void OnEnable()
    {
        timeCounter = 0f;

        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.TaraTalkingBackground);
        AudioManager.instance.StartPlayingFromAudioManager(transformSFX);
        AudioManager.instance.StartPlayingFromAudioManager(transformBackground);

        monsterFootstep = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.HeavyFootstep2);
        monsterSound0 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraSpeechAggresive3);
        monsterAttack0 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraAttack0);
        monsterAttack1 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.TaraAttack1);
        /*
        monsterSound1 = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterAttack);
        monsterScream = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.MonsterBreathe);
        monsterImpact = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.OpenDoor);
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        transformationFinishedCinematic.played += LockPlayerControl;
        transformationFinishedCinematic.stopped += UnlockPlayerControl;

        int idle = (int) monsterStatesEnm.transform_idle;
        myAnimator.SetTrigger(((int)idle).ToString());
    }

    [Header("Settings")]
    [SerializeField] float beginTransformationCinematicDelayTimePoint = 3f;
    [SerializeField] float timeForTransformation = 15f;
    float timeCounter = 0f;
    // Update is called once per frame
    void Update()
    {
        if(lastState != currentState)
        {
            myAnimator.SetTrigger(((int)currentState).ToString());
        }

        lastState = currentState;

        switch (currentState)
        {
            case monsterStatesEnm.transform_idle:

                //Wait for specific time
                timeCounter += Time.deltaTime;
                if(transformationFinishedCinematic.state != PlayState.Playing)
                {
                    if (timeCounter > (timeForTransformation - beginTransformationCinematicDelayTimePoint))
                    {
                        transformationFinishedCinematic.Play();
                    }
                }
                if (timeCounter > timeForTransformation)
                {
                    timeCounter = 0f;
                    currentState = monsterStatesEnm.transform_finish;
                }
                break;
            case monsterStatesEnm.transform_finish:
                //Wait for transformationFinished animationEvent
                //AudioManager.instance.StopFromAudioManager(transformSFX);
                if (transformationFinished)
                {
                    transform.Rotate(Vector3.up, 180);
                    currentState = monsterStatesEnm.walk;
                }
                break;
            case monsterStatesEnm.walk:
                //use NavMeshInstead?
                MonsterMove(runSpeed);

                if (Vector3.Distance(transform.position, playerTransform.position) <= minimalDistanceToPlayer)
                {
                    currentState = monsterStatesEnm.kill;
                    MakePlayerLookAtMonster();
                }

                break;

            case monsterStatesEnm.kill:
                TriggerBloodEffect();

                if (finishedChew >= 1)
                {
                    currentState = monsterStatesEnm.end_of_state;
                    BadEndGameTrigger();
                }
                break;
            default:
                break;
        }
    }

    private void MonsterMove(float speed)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }

    internal Vector3 GetLookAtPoint()
    {
        return lookAtPosition.position;
    }

    public void SetMonsterState(monsterStatesEnm state)
    {
        currentState = state;
        if(state == monsterStatesEnm.idle)
        {
            transform.position = endPosition.position;
        }
    }

    [SerializeField] GameObject BloodEffectCanvas = null;
    float BloodTimer = 0f;
    private void TriggerBloodEffect()
    {
        if (BloodTimer > .25f)
        {
            if (BloodEffectCanvas) BloodEffectCanvas.SetActive(!BloodEffectCanvas.activeSelf);
            BloodTimer = 0f;
            return;
        }
        BloodTimer += Time.deltaTime;
    }
    private void MakePlayerLookAtMonster()
    {
        Debug.Log("LockMenuControl");
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();
        Vector3 LookAtPlayer = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z); //to use the same ground
        transform.LookAt(LookAtPlayer);
        FindObjectOfType<MouseLook>().LockAndLookAtPoint(GetLookAtPoint()); //lock camera and player movement
        //transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    private void BadEndGameTrigger()
    {
        Debug.Log("Game has been ended with BAD END! :-)");
        //make something to end the game
        //currently just disables monster
        AudioManager.instance.StopAllSounds();
        //LevelLoader.instance.ending = Ending.Bad;

        StartCoroutine(LevelLoader.instance.StartLoadingSameScene(2f));
        //currentState = monsterStatesEnm.end_of_state;
        //StartCoroutine(LevelLoader.instance.StartLoadingSameScene(2f));
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
        if(pd != null) transformationFinished = true;

        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();
        FindObjectOfType<MouseLook>().UnlockFromPoint();
        FindObjectOfType<PlayerMovement>().UnlockPlayer();
    }
}
