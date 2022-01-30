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
        FindObjectOfType<PlayerMovement>().runEnable = false;
        GetComponent<OpeningDoor>().OnDoorOpened += RevealMonsterStart;
    }

    public void RevealMonsterStart()
    {
        StartCoroutine(RevealMonster());
    }


    public IEnumerator RevealMonster()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("RevealMonster");

        monsterToReveal.SetActive(true);
        FindObjectOfType<MouseLook>().LockAndLookAtPoint(monsterToReveal.GetComponent<EndlessTrainMonsterCntrl>().GetLookAtPoint());

        FindObjectOfType<DialogueUI>().ShowTutorialBox(0);
        FindObjectOfType<PlayerMovement>().runEnable = true;

        yield return new WaitForSeconds(2.5f);

        FindObjectOfType<MouseLook>().UnlockFromPoint();

    }
}
