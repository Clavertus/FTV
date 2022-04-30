using FTV.Saving;
using FTV.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaraMementoManager : MonoBehaviour, ISaveable
{
    public enum currentMementoEnum
    {
        memento_book,
        memento_jar,
        memento_box,
        memento_move_to_next_scene
    }
    currentMementoEnum memento_state = currentMementoEnum.memento_book;

    [Header("Mementos")]
    [SerializeField] GameObject memento_book = null;
    [SerializeField] GameObject memento_jar = null;
    [SerializeField] GameObject memento_box = null;

    [Header("Dialogs")]
    [SerializeField] NPCDialogue dialogToPlayOnStart = null;
    [SerializeField] NPCDialogue dialogToPlayOnTrigger = null;
    [SerializeField] NPCDialogue dialogToPlayOnJarMemento = null;
    [SerializeField] NPCDialogue dialogToPlayOnBoxMemento = null;

    [Header("Other")]
    [SerializeField] Transform startPosition = null;
    [SerializeField] PlayerMovement player = null;
    [SerializeField] GameObject triggerZone = null;
    [SerializeField] GameObject DoorToClose = null;
    [SerializeField] Transform DoorClosePosition = null;
    [SerializeField] TrainEffectController trainToChange = null;
    [SerializeField] Transform doorToDisable = null;
    [SerializeField] Transform trainToRevealAndHide = null;
    private bool dialogOnStart = true;
    private bool triggerZoneTriggered = false;
    private DialogueUI dialogUI = null;

    // Start is called before the first frame update
    void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
        dialogUI.OnDialogShowEnd += OnDialogFinished;

        if (LevelLoader.instance.GetSavedTransform() != null)
        {
            startPosition.eulerAngles = LevelLoader.instance.GetSavedTransform().eulerAngles;
            startPosition.position = LevelLoader.instance.GetSavedTransform().position;
        }
        else
        {
            startPosition.eulerAngles = player.transform.eulerAngles;
            startPosition.position = player.transform.position;
        }

        SetPlayerToSavedTransform();
        AudioManager.instance.StopFromAudioManager(soundsEnum.TaraTalkingBackground);
        AudioManager.instance.StartPlayingFromAudioManager(soundsEnum.JarLoop);
    }

    // Update is called once per frame
    void Update()
    {

        if (dialogOnStart == true)
        {
            dialogOnStart = false;
            dialogUI.ShowDialogue(dialogToPlayOnStart);
        }

        if(triggerZone.activeSelf)
        {
            if(triggerZoneTriggered)
            {
                memento_state = currentMementoEnum.memento_jar;
                triggerZone.SetActive(false);
            }
        }
    }

    public IEnumerator DoorTriggered()
    {
        player.LockPlayer();
        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<InGameMenuCotrols>().LockMenuControl();

        yield return LevelLoader.instance.FadeIn();


        DoorToClose.transform.position = DoorClosePosition.position;
        DoorToClose.GetComponent<OpeningDoor>().ResetToDefaultState();

        FindObjectOfType<TriggerLoop>().SetInteractionCounterTo(0);

        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.Change5);

        SetPlayerToSavedTransform();

        if (memento_state >= currentMementoEnum.memento_jar)
        {
            memento_jar.SetActive(true);
            trainToChange.setTrainMaterial(TrainEffectController.trainMaterialType.rusty00);
        }

        if (memento_state >= currentMementoEnum.memento_box)
        {
            memento_box.SetActive(true);
            trainToChange.setTrainMaterial(TrainEffectController.trainMaterialType.rusty01);
        }

        yield return LevelLoader.instance.FadeOut();

        player.UnlockPlayer();
        FindObjectOfType<MouseLook>().UnlockCamera();
        FindObjectOfType<InGameMenuCotrols>().UnlockMenuControl();

        if (memento_state >= currentMementoEnum.memento_box)
        {
            trainToRevealAndHide.gameObject.SetActive(false);
            doorToDisable.GetComponent<Selectable>().enabled = false;
            doorToDisable.GetComponent<OpeningDoor>().enabled = false;
        }
    }

    private void OnDialogFinished(FTV.Dialog.NPCDialogue dialog)
    {
        if (dialog == dialogToPlayOnStart)
        {
            SavingWrapper.instance.CheckpointSave();
        }

        else if (dialog == dialogToPlayOnTrigger)
        {
            triggerZoneTriggered = true;
        }

        else if (dialog == dialogToPlayOnJarMemento)
        {
            memento_state = currentMementoEnum.memento_box;
        }

        else if (dialog == dialogToPlayOnBoxMemento)
        {
            memento_state = currentMementoEnum.memento_move_to_next_scene;
            StartCoroutine(LevelLoader.instance.StartLoadingNextScene());
            AudioManager.instance.StopFromAudioManager(soundsEnum.JarLoop);
        }
    }

    public void SetMementoState(currentMementoEnum new_state)
    {
        memento_state = new_state;
    }

    private void SetPlayerToSavedTransform()
    {
        player.enabled = false;
        while (player.transform.position != startPosition.position)
        {
            player.transform.eulerAngles = startPosition.eulerAngles;
            player.transform.position = startPosition.position;
        }
        player.enabled = true;
        Debug.Log("SetPlayerToSavedTransform");
    }

    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public int memento_state;
        public bool dialogOnStart;
        public bool triggerZoneTriggered;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.dialogOnStart = dialogOnStart;
        data.triggerZoneTriggered = triggerZoneTriggered;
        data.memento_state = (int)memento_state;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        dialogOnStart = data.dialogOnStart;
        triggerZoneTriggered = data.triggerZoneTriggered;
        memento_state = (currentMementoEnum)data.memento_state;
    }
    #endregion
}
