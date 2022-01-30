using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMonsterReveal : MonoBehaviour
{
    [SerializeField] GameObject monsterToReveal = null;

    private void Start()
    {
        GetComponent<OpeningDoor>().OnDoorOpened += RevealMonster;
    }

    public void RevealMonster()
    {
        Debug.Log("RevealMonster");
        monsterToReveal.SetActive(true);
        FindObjectOfType<MouseLook>().LockAndLookAtPoint(monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().GetLookAtPoint());

        FindObjectOfType<DialogueUI>().ShowTutorialBox(0);
    }
}
