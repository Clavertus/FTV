using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FTV.Dialog;
public class CultistBehaviour : MonoBehaviour
{
    [SerializeField] NPCDialogue dialogWithCultistTaraMonster = null;
    [SerializeField] NPCDialogue dialogWithCultistTaraHuman = null;
    [SerializeField] PlayNPCDialog npc_Dialog = null;

    private AudioSource[] cultistSpeech = new AudioSource[5];
    private void OnEnable()
    {
        if(ProgressTracker.instance)
        {
            if(ProgressTracker.instance.taraEnding == ProgressTracker.endingType.Good)
            {
                npc_Dialog.SetNewDialogAvailableAndPlay(dialogWithCultistTaraHuman);
            }
            else
            {
                npc_Dialog.SetNewDialogAvailableAndPlay(dialogWithCultistTaraMonster);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cultistSpeech[0] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.CultistSpeech1);
        cultistSpeech[1] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.CultistSpeech2);
        cultistSpeech[2] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.CultistSpeech3);
        cultistSpeech[3] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.CultistSpeech4);
        cultistSpeech[4] = AudioManager.instance.AddAudioSourceWithSound(gameObject, soundsEnum.CultistSpeech5);

        npc_Dialog.PreTriggerEventCall += OnPreTriggerEventCall;
        npc_Dialog.DialogIsStarted += OnDialogStarted;
        npc_Dialog.DialogIsFinished += OnDialogFinished;
        npc_Dialog.DialogNodeIsStarted += OnDialogNodeStarted;
        npc_Dialog.DialogNodeIsEnded += OnDialogNodeFinished;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPreTriggerEventCall()
    {
        //AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.TaraTalkingBackground);
    }

    private void EndPreTriggerEvent()
    {
        npc_Dialog.PreTriggerEventFinished();
    }

    private void OnDialogStarted()
    {
        //AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.TaraTalkingBackground);
    }

    int dialogId = 0;
    private void OnDialogNodeStarted(int triggerId)
    {
        Debug.Log("Dialog with id: " + triggerId);
        //speaking = true;
        AudioManager.instance.InstantPlayFromGameObject(cultistSpeech[dialogId]);
        dialogId += 1;
        if (dialogId >= cultistSpeech.Length) dialogId = 0;
    }
    private void OnDialogNodeFinished(int triggerId)
    {
        Debug.Log("Dialog with id: " + triggerId);
    }

    private void OnDialogFinished()
    {
        //next scene
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
    }
}
