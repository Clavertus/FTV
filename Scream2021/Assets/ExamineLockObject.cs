using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineLockObject : MonoBehaviour
{
    public Action Unlocked { get; set; }

    [SerializeField] Canvas LockHintUiCanvas = null;
    [SerializeField] Canvas LockInterfaceUiCanvas = null;
    [SerializeField] GameObject[] SlotBackgrounds = null;
    [SerializeField] GameObject[] SlotImages = null;
    [SerializeField] Sprite[] imageList = null;

    [SerializeField] Color slotUnselected = Color.white;
    [SerializeField] Color slotSelected = Color.white;

    [SerializeField] int[] slotValuesArray = null;
    [SerializeField] int[] unlockSequence = null;
    private bool unlocked = false;

    int currentSlotId = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (SlotBackgrounds.Length == 0)
        {
            Debug.LogError("No slots assigned at all!");
            return;
        }
        if (SlotImages.Length == 0)
        {
            Debug.LogError("No slots assigned at all!");
            return;
        }
        if(SlotImages.Length != SlotBackgrounds.Length)
        {
            Debug.LogError("Slots and images of them should have the same length");
            return;
        }
        if (unlockSequence.Length != SlotImages.Length)
        {
            Debug.LogError("Unlock Sequence Length and Slots Number of them should have the same number");
            return;
        }
        if (slotValuesArray.Length != SlotImages.Length)
        {
            Debug.LogError("slotValuesArray Length and Slots Number of them should have the same number");
            return;
        }

        foreach (int value in slotValuesArray)
        {
            if (value < 0 && value >= imageList.Length)
            {
                Debug.LogError("Invalid value assigned as unlockSequence!");
            }
        }
        foreach ( int value in unlockSequence)
        {
            if(value < 0 && value >= imageList.Length)
            {
                Debug.LogError("Invalid value assigned as unlockSequence!");
            }
        }

        LockHintUiCanvas.gameObject.SetActive(false);
        currentSlotId = 0;
        //initialise images
        for (int ix = 0; ix < SlotImages.Length; ix++)
        {
            SlotImages[ix].GetComponent<Image>().sprite = imageList[slotValuesArray[ix]];
        }
    }

    bool inspectedOnce = false;
    // Update is called once per frame
    void Update()
    {
        if (inspectedOnce == false)
        {
            return;
        }

        if (false == unlocked)
        {
            ProcessSelectionAndUnlocking();
        }
        else
        {
            //?
        }
    }

    private void ProcessSelectionAndUnlocking()
    {
        if (gameObject.CompareTag("Selectable"))
        {
            SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotSelected;
            //LockInterfaceUiCanvas.gameObject.SetActive(true);
            LockHintUiCanvas.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.A))
            {
                SelectNextSlotInPositiveDirection(false);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SelectNextSlotInPositiveDirection(true);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                SelectNextImageInPositiveDirection(true);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                SelectNextImageInPositiveDirection(false);
            }

            int unlockedValue = 0;
            for(int ix = 0; ix < slotValuesArray.Length; ix++)
            {
                if(slotValuesArray[ix] == unlockSequence[ix])
                {
                    unlockedValue++;
                }
            }

            if(unlockedValue == unlockSequence.Length)
            {
                unlocked = true;
                SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotUnselected;
                Unlocked?.Invoke();
            }
        }
        else if (gameObject.CompareTag("Untagged"))
        {
            SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotUnselected;
            //LockInterfaceUiCanvas.gameObject.SetActive(false);
            LockHintUiCanvas.gameObject.SetActive(false);
        }
    }

    public bool IsUnlocked()
    {
        return unlocked;
    }

    public void InspectedOnce()
    {
        inspectedOnce = true;
        SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotSelected;
        GetComponent<Selectable>().ChangeUi(LockHintUiCanvas);
        LockInterfaceUiCanvas.gameObject.SetActive(true);
        LockHintUiCanvas.gameObject.SetActive(true);
        gameObject.tag = "Selectable";
    }

    public void SelectNextSlotInPositiveDirection(bool positiveDirection)
    {
        if(SlotBackgrounds.Length == 0)
        {
            Debug.LogError("No slots assigned at all!");
            return;
        }

        int nextSlotId = currentSlotId;
        if(positiveDirection)
        {
            nextSlotId++;
            if (nextSlotId >= SlotBackgrounds.Length)
            {
                nextSlotId = 0;
            }
        }
        else
        {
            nextSlotId--;
            if(nextSlotId < 0)
            {
                nextSlotId = SlotBackgrounds.Length - 1;
            }
        }

        SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotUnselected;
        SlotBackgrounds[nextSlotId].GetComponent<Image>().color = slotSelected;

        currentSlotId = nextSlotId;
    }

    public void SelectNextImageInPositiveDirection(bool positiveDirection)
    {
        if (SlotImages.Length == 0)
        {
            Debug.LogError("No slots assigned at all!");
            return;
        }

        int nextSlotValue = slotValuesArray[currentSlotId];
        if (positiveDirection)
        {
            nextSlotValue++;
            if (nextSlotValue >= imageList.Length)
            {
                nextSlotValue = 0;
            }
        }
        else
        {
            nextSlotValue--;
            if (nextSlotValue < 0)
            {
                nextSlotValue = imageList.Length - 1;
            }
        }

        SlotImages[currentSlotId].GetComponent<Image>().sprite = imageList[nextSlotValue];
        slotValuesArray[currentSlotId] = nextSlotValue;
    }
}
