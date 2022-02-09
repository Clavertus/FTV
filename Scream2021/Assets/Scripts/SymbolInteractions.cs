using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class SymbolInteractions : MonoBehaviour, ISaveable
{
    [Header("Dialogue Data")]
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject dPadDialogue;

    [SerializeField] DialogueObject zipperDialogue;
    [SerializeField] DialogueObject frameStandDialogue;
    [SerializeField] DialogueObject doorUnlocked;

    [SerializeField] DialogueObject InspectAfter1stInteractionDialogue;
    [SerializeField] DialogueObject InspectAfter2stInteractionDialogue;
    [SerializeField] DialogueObject InspectAfter3stInteractionDialogue;
    [SerializeField] DialogueObject InspectAfter4stInteractionDialogue;

    [Header("GameObject references")]

    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject gameBoyMemento;
    [SerializeField] GameObject zipperMemento;
    [SerializeField] GameObject photoMemento;

    [SerializeField] GameObject dPadClone;
    [SerializeField] GameObject zipperClone;
    [SerializeField] GameObject elderGodMove;
    [SerializeField] GameObject secondTrainMove; 


    [SerializeField] GameObject chain;
    [SerializeField] GameObject chainDPad;

    internal void ShowChain()
    {
        GetComponent<Selectable>().enabled = true;
        chain.SetActive(true);
    }

    [SerializeField] GameObject chainZip;

    [SerializeField] GameObject trainMonster;

    [SerializeField] GameObject doorToCar; 

    [Header("Symbol Materials")] 

    [SerializeField] Material dPadMat;
    [SerializeField] Material zipperMat;
    [SerializeField] Material frameMat;

    


    bool pocketed = false;
    bool checkedWindows = false;

    AudioSource myAudioSource; 

    string pocketItem;
    int interactionCounter = 0;
   
    void Start()
    {
        GetComponent<Selectable>().DisableSelectable();
        if(checkpointArrived)
        {
            AfterSave();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == ("Selected"))
        {
            switch(interactionCounter)
            {
                case 0:
                    FirstInteraction();
                    break;
                case 1:
                    if (pocketed && pocketItem == ("DPad"))
                    {
                        ApplyDPad();
                    }
                    else
                    {
                        FindObjectOfType<DialogueUI>().ShowDialogue(InspectAfter1stInteractionDialogue);
                        gameObject.tag = ("Untagged");
                    }
                        break;
                case 2:
                    if (pocketed && pocketItem == ("Zipper"))
                    {
                        ApplyZipper();
                    }
                    else
                    {
                        FindObjectOfType<DialogueUI>().ShowDialogue(InspectAfter2stInteractionDialogue);
                        gameObject.tag = ("Untagged");
                    }
                    break;
                case 3:
                    if (pocketed && pocketItem == ("Frame Stand"))
                    {
                        StartCoroutine(ApplyFrameStand());
                    }
                    else
                    {
                        FindObjectOfType<DialogueUI>().ShowDialogue(InspectAfter3stInteractionDialogue);
                        gameObject.tag = ("Untagged");
                    }
                    break;
                default:
                    FindObjectOfType<DialogueUI>().ShowDialogue(InspectAfter4stInteractionDialogue);
                    gameObject.tag = ("Untagged");
                    break;
            }
        }
    }

    public void AreWindowsChecked() { checkedWindows = true; }
    public void IsPocketed(string pocketedItem) { pocketed = true; pocketItem = pocketedItem; } 
    void FirstInteraction()
    {
        gameObject.tag = ("Untagged");
        FindObjectOfType<DialogueUI>().ShowDialogue(firstDialogue);
        gameBoyMemento.SetActive(true);
        interactionCounter++;
    }

    void ApplyDPad()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.ApplyDPad);
        FindObjectOfType<DialogueUI>().ShowDialogue(dPadDialogue);
         
        gameObject.tag = ("Untagged");

        chainDPad.GetComponent<MeshRenderer>().material = dPadMat;
        interactionCounter++;
        zipperMemento.SetActive(true);
        gameBoyMemento.SetActive(false);
    }

    void ApplyZipper()
    {
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.ApplyZipper);

        gameObject.tag = ("Untagged");

        elderGodMove.SetActive(true);
        elderGodMove.GetComponent<GodPointMovement>().increaseSpeed();

        chainZip.GetComponent<MeshRenderer>().material = zipperMat; 
        secondTrainMove.SetActive(true);
        FindObjectOfType<DialogueUI>().ShowDialogue(zipperDialogue);
        photoMemento.SetActive(true);
        zipperMemento.SetActive(false); 
        interactionCounter++;
    }

    IEnumerator ApplyFrameStand() 
    {
        AudioManager.instance.PlayOneShotFromAudioManager(soundsEnum.ApplyFrameStand);

        gameObject.tag = ("Untagged");

        elderGodMove.GetComponent<GodPointMovement>().SetToPointB();

        photoMemento.SetActive(false);
        chain.GetComponent<MeshRenderer>().material = frameMat;
        FindObjectOfType<DialogueUI>().ShowDialogue(frameStandDialogue);
        interactionCounter++;


        yield return new WaitUntil(() => !dialogueBox.activeSelf);
        //save system
        checkpointArrived = true;
        FindObjectOfType<SavingWrapper>().CheckpointSave();
        UnlockDoor();
    }

    void UnlockDoor()
    {
        elderGodMove.GetComponent<GodPointMovement>().increaseSpeed();

        doorToCar.GetComponent<Selectable>().enabled = true;
        doorToCar.GetComponent<DoorToCar>().UnlockDoorSFX(); 
        FindObjectOfType<SecondTrain>().TriggerTrain();
        FindObjectOfType<DialogueUI>().ShowDialogue(doorUnlocked);
    }

    void AfterSave()
    {
        GetComponent<Selectable>().enabled = true;
        chain.SetActive(true);
        chainDPad.GetComponent<MeshRenderer>().material = dPadMat;
        chainZip.GetComponent<MeshRenderer>().material = zipperMat;
        chain.GetComponent<MeshRenderer>().material = frameMat;

        elderGodMove.SetActive(true);
        elderGodMove.GetComponent<GodPointMovement>().increaseSpeed();
        elderGodMove.GetComponent<GodPointMovement>().SetToPointB();
        elderGodMove.GetComponent<GodPointMovement>().increaseSpeed();

        secondTrainMove.SetActive(true);
        doorToCar.GetComponent<Selectable>().enabled = true;
        doorToCar.GetComponent<DoorToCar>().UnlockDoorSFX();
        FindObjectOfType<SecondTrain>().TriggerTrain();

        //FindObjectOfType<DialogueUI>().ShowDialogue(doorUnlocked);
    }

    bool checkpointArrived = false;
    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
        public bool checkpointArrived;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        data.checkpointArrived = checkpointArrived;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;
        checkpointArrived = data.checkpointArrived;
        if(checkpointArrived)
        {
        }
    }
}
