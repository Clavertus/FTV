using FTV.Saving;
using FTV.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenByKey : MonoBehaviour, ISaveable
{
    [SerializeField] NPCDialogue noKeyDialog = null;
    [SerializeField] GameObject KeyObjectInventory = null;
    [SerializeField] GameObject revealObject = null;
    [SerializeField] Transform shelfDoor = null;
    [SerializeField] float rotateToAngle = -90;
    [SerializeField] float rotateSpeedInSec = -90;
    int interactionCounter = 0;

    private void Start()
    {
        if (rotateToAngle < 0)
        {
            rotateToAngle = 360 - Mathf.Abs(rotateToAngle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Selected" && interactionCounter == 0) { StartCoroutine(Interaction()); return; }

        Rotate();
    }

    bool rotateToOpen = false;
    private IEnumerator Interaction()
    {
        gameObject.tag = ("Selectable");
        interactionCounter++;
        if (KeyObjectInventory.activeSelf)
        {
            revealObject.SetActive(true);
            KeyObjectInventory.SetActive(false);
            gameObject.tag = ("Untagged");
            this.GetComponent<Selectable>().enabled = false;
            rotateToOpen = true;
        }
        else
        {
            if (noKeyDialog) FindObjectOfType<DialogueUI>().ShowDialogue(noKeyDialog);
            yield return new WaitUntil(() => !FindObjectOfType<DialogueUI>().dialogueBox.activeSelf);
            interactionCounter--;
        }
    }

    private void Rotate()
    {
        if (rotateToOpen)
        {
            Debug.Log(rotateToAngle);
            Debug.Log(shelfDoor.localEulerAngles.y);
            if ((shelfDoor.localEulerAngles.y <= rotateToAngle + 10f) && (shelfDoor.localEulerAngles.y >= rotateToAngle - 10f))
            {
                shelfDoor.localEulerAngles = new Vector3(
                    shelfDoor.localEulerAngles.x,
                    rotateToAngle,
                    shelfDoor.localEulerAngles.z
                    );
            }
            else
            {
                shelfDoor.localEulerAngles = new Vector3(
                    shelfDoor.localEulerAngles.x,
                    shelfDoor.localEulerAngles.y + rotateSpeedInSec * Time.deltaTime,
                    shelfDoor.localEulerAngles.z
                    );

                if ((shelfDoor.localEulerAngles.y <= rotateToAngle + 5f) && (shelfDoor.localEulerAngles.y >= rotateToAngle - 5f))
                {
                    shelfDoor.localEulerAngles = new Vector3(
                        shelfDoor.localEulerAngles.x,
                        rotateToAngle,
                        shelfDoor.localEulerAngles.z
                        );
                }
            }
        }
    }

    [System.Serializable]
    struct SaveData
    {
        public int interactionCounter;
        public bool rotateToOpen;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.interactionCounter = interactionCounter;
        data.rotateToOpen = rotateToOpen;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        interactionCounter = data.interactionCounter;
        rotateToOpen = data.rotateToOpen;
        if (rotateToOpen)
        {
            shelfDoor.localEulerAngles = new Vector3
                (
                shelfDoor.localEulerAngles.x,
                rotateToAngle,
                shelfDoor.localEulerAngles.z
                );
        }
    }
}
