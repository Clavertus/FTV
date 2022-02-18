using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineLockObject : MonoBehaviour
{
    [SerializeField] Canvas LockHintUiCanvas = null;
    [SerializeField] Canvas LockInterfaceUiCanvas = null;
    [SerializeField] GameObject[] SlotBackgrounds = null;
    [SerializeField] GameObject[] SlotImages = null;
    [SerializeField] Sprite[] imageList = null;
    int[] slotValuesArray = null;

    [SerializeField] Color slotUnselected = Color.white;
    [SerializeField] Color slotSelected = Color.white;



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

        slotValuesArray = new int[SlotImages.Length];
        for(int ix = 0; ix < slotValuesArray.Length; ix++)
        {
            slotValuesArray[ix] = UnityEngine.Random.Range(0, imageList.Length-1);
            SlotImages[ix].GetComponent<Image>().sprite = imageList[slotValuesArray[ix]];
        }

        currentSlotId = 0;
        SlotBackgrounds[currentSlotId].GetComponent<Image>().color = slotSelected;

        LockInterfaceUiCanvas.gameObject.SetActive(false);
        LockHintUiCanvas.gameObject.SetActive(false);
    }

    bool inspectedOnce = false;
    // Update is called once per frame
    void Update()
    {
        if (inspectedOnce == false)
        {
            return;
        }

        if (gameObject.CompareTag("Selectable"))
        {
            LockInterfaceUiCanvas.gameObject.SetActive(true);
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
        }
        else if (gameObject.CompareTag("Untagged"))
        {
            LockInterfaceUiCanvas.gameObject.SetActive(false);
            LockHintUiCanvas.gameObject.SetActive(false);
        }
    }

    public void InspectedOnce()
    {
        inspectedOnce = true;
        LockInterfaceUiCanvas.gameObject.SetActive(true);
        LockHintUiCanvas.gameObject.SetActive(true);
        GetComponent<Selectable>().ChangeUi(LockHintUiCanvas);
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
