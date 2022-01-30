using System.Collections; 
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("dialog box")]
    [SerializeField] TMP_Text dialogueBoxTextLabel;
    [SerializeField] public GameObject dialogueBox;

    [Header("choose box")]
    [SerializeField] GameObject chooseBox;
    [SerializeField] TMP_Text[] chooseBox_TextLabels;

    [SerializeField] Image dialogueBox_image = null;
    [SerializeField] FTV.Dialog.DialogStyle defaultStyle = null;

    [Header("tutorial box")]
    [SerializeField] GameObject tutorialBox;
    [SerializeField] GameObject[] tutorialBox_Messages;
    [SerializeField] float fadeStep = .5f;

    DisplayDialogue displayDialogue;
    private int selectedDialogueId;
    private CanvasGroup tutorialGroup = null;
    private void Start()
    {
        displayDialogue = GetComponent<DisplayDialogue>();

        chooseBox.SetActive(false);
        tutorialBox.SetActive(false);
        foreach(var message in tutorialBox_Messages) message.SetActive(false);
        tutorialGroup = tutorialBox.GetComponent<CanvasGroup>();
        tutorialGroup.alpha = 0f;

        CloseDialogueBox();

        if(dialogueBox == null)
        {
            Debug.LogError("dialogBox is not assigned!");
        }
    }

    private void Update()
    {
        if(tutorialBox.activeSelf)
        {
            ShowSelectedTutorialBox();
        }
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);

        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer(); 
        FindObjectOfType<SelectionManager>().LockSelection();
        
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    public void ShowDialogue(FTV.Dialog.NPCDialogue dialogueObject)
    {
        dialogueBox.SetActive(true);

        FindObjectOfType<MouseLook>().LockCamera();
        FindObjectOfType<PlayerMovement>().LockPlayer();
        FindObjectOfType<SelectionManager>().LockSelection();

        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        dialogueBox_image.sprite = defaultStyle.GetImage();
        dialogueBox_image.color = defaultStyle.GetColor();
        foreach (string dialogue in dialogueObject.Dialogue) 
        {
            //call run method in displayDialogue, passing in each dialogue in the dialogue object
            yield return displayDialogue.Run(dialogue, dialogueBoxTextLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));  
        }
        CloseDialogueBox();
    }
    public IEnumerator StepThroughDialogue(FTV.Dialog.NPCDialogue dialogueObject)
    {
        bool exitDialogue = false;
        FTV.Dialog.DialogNode nextDialogue = dialogueObject.GetRootNode();

        while (!exitDialogue)
        {
            if (nextDialogue.GetDialogStyle())
            {
                dialogueBox_image.sprite = nextDialogue.GetDialogStyle().GetImage();
                dialogueBox_image.color = nextDialogue.GetDialogStyle().GetColor();
            }
            else
            {
                dialogueBox_image.sprite = defaultStyle.GetImage();
                dialogueBox_image.color = defaultStyle.GetColor();
            }

            //call run method in displayDialogue, passing in each dialogue in the dialogue object
            yield return displayDialogue.Run(nextDialogue.GetText(), dialogueBoxTextLabel);
            if(nextDialogue.GetChildren().Count > 1)
            {
                //choose what children to display?
                FTV.Dialog.DialogNode dialogZero = dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[0]);
                if (dialogZero.GetIsPlayerSpeaking())
                {
                    chooseBox.SetActive(true);

                    foreach (TMP_Text textLabel in chooseBox_TextLabels)
                    {
                        textLabel.transform.parent.gameObject.SetActive(false);
                    }

                    int ix = 0;
                    foreach(FTV.Dialog.DialogNode children in dialogueObject.GetAllChildren(nextDialogue))
                    {
                        chooseBox_TextLabels[ix].transform.parent.gameObject.SetActive(true);
                        Debug.Log(chooseBox_TextLabels[ix].transform.parent.gameObject.name);
                        chooseBox_TextLabels[ix].SetText(dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[ix]).GetText());
                        ix++;
                    }

                    // if it is a player -> player chooses the what to say
                    yield return new WaitUntil(() => PlayerChooseDialogue(dialogueObject, nextDialogue));

                    chooseBox.SetActive(false);
                    nextDialogue = dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[selectedDialogueId]);
                    //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                }
                else
                {
                    // if it is an NPC -> random? or always the first one
                    int randomDialogId = Random.Range((int)0, (int)(nextDialogue.GetChildren().Count));
                    nextDialogue = dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[randomDialogId]);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                }
            }
            else if(nextDialogue.GetChildren().Count == 1)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                nextDialogue = dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[0]);
            }
            else
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                exitDialogue = true;
            }

        }
        CloseDialogueBox();
    }

    public bool PlayerChooseDialogue(FTV.Dialog.NPCDialogue dialogueObject, FTV.Dialog.DialogNode parentNode)
    {
        //some choose UI (buttons?)

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedDialogueId = 0;
            return true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && (parentNode.GetChildren().Count > 1))
        {
            selectedDialogueId = 1;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && (parentNode.GetChildren().Count > 2))
        {
            selectedDialogueId = 2;
            return true;
        }
        return false;
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        dialogueBoxTextLabel.text = string.Empty;

        FindObjectOfType<MouseLook>().UnlockCamera();
        FindObjectOfType<PlayerMovement>().UnlockPlayer(); 
        FindObjectOfType<SelectionManager>().UnlockSelection();
    }

    private bool showTutorialBox = true;
    private int showMessageId = 0;
    public void ShowTutorialBox(int messageId)
    {
        Debug.Log("Show tutorial box");
        if (messageId >= tutorialBox_Messages.Length) return;

        showTutorialBox = true;
        showMessageId = messageId;

        tutorialBox.SetActive(true);
        tutorialBox_Messages[showMessageId].SetActive(true);
    }

    private void ShowSelectedTutorialBox()
    {
        if(showTutorialBox)
        {
            if(tutorialGroup.alpha < 1f)
            {
                tutorialGroup.alpha += fadeStep * Time.deltaTime;
            }
            else
            {
                showTutorialBox = false;
            }
        }
        else
        {
            if (tutorialGroup.alpha > 0.0f)
            {
                tutorialGroup.alpha -= fadeStep * Time.deltaTime;
            }
            else
            {
                showTutorialBox = false;
                tutorialBox_Messages[showMessageId].SetActive(false);
                tutorialBox.SetActive(false);
            }
        }
    }
    
}
