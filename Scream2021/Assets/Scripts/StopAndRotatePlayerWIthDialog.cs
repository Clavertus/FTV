using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTV.Dialog;
using System;

public class StopAndRotatePlayerWIthDialog : MonoBehaviour
{
    [SerializeField] NPCDialogue dialogToPlay = null;
    [SerializeField] float offsetXToMovePlayer = 0f;
    [SerializeField] float offsetZToMovePlayer = 0f;

    Transform playerTransform = null;
    DialogueUI dialogUI = null;
    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        dialogUI = FindObjectOfType<DialogueUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other) return;

        if(other.tag == "Player")
        {
            StartCoroutine(StartPlayDialog());
        }
    }

    private IEnumerator StartPlayDialog()
    {
        if (dialogToPlay)
        {
            dialogUI.ShowDialogue(dialogToPlay);
        }
        yield return null;
    }
}
