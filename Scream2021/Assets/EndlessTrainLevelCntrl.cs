using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainLevelCntrl : MonoBehaviour
{
    [SerializeField] FTV.Dialog.NPCDialogue dialogue0 = null;
    [SerializeField] FTV.Dialog.NPCDialogue dialogue1 = null;
    private bool dialogue0_played = false;
    private bool dialogue1_played = false;

    [SerializeField] float callDialog0After = 2f;
    [SerializeField] float callDialog1After = 20f;
    private float timeCounter = 0f;

    private DialogueUI dialogUI = null;
    void Awake()
    {
        TrainEffectController[] trains = FindObjectsOfType<TrainEffectController>();
        foreach (TrainEffectController train in trains)
        {
            train.SetPosterMatId(1);
        }
    }

    private void Start()
    {
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    private void Update()
    {
        if(timeCounter > callDialog0After && !dialogue0_played)
        {
            dialogue0_played = true;
            //play the first dialog
            dialogUI.ShowDialogue(dialogue0);
        }

        if (timeCounter > callDialog1After && !dialogue1_played)
        {
            dialogue1_played = true;
            //play the first dialog
            dialogUI.ShowDialogue(dialogue1);
        }

        timeCounter += Time.deltaTime;

        if (dialogUI.dialogueBox.activeSelf)
        {
            timeCounter = 0f;
        }
    }
}
