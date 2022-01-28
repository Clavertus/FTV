using System.Collections; 
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueBoxTextLabel;
    [SerializeField] DialogueObject testDialogue;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] GameObject chooseBox;
    [SerializeField] TMP_Text chooseBox_TextLabel_0;
    [SerializeField] TMP_Text chooseBox_TextLabel_1;

    [SerializeField] Image dialogueBox_image = null;
    [SerializeField] FTV.Dialog.DialogStyle defaultStyle = null;

    DisplayDialogue displayDialogue;
    private int selectedDialogueId;

    private void Start()
    {
        displayDialogue = GetComponent<DisplayDialogue>();

        chooseBox.SetActive(false);

        CloseDialogueBox();
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

                    chooseBox_TextLabel_0.SetText(dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[0]).GetText());
                    chooseBox_TextLabel_1.SetText(dialogueObject.GetSpecificChildren(nextDialogue, nextDialogue.GetChildren()[1]).GetText());

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

        /*
        foreach (FTV.Dialog.DialogNode dialogue in dialogueObject.GetAllNodes())
        {
            if(dialogue.GetDialogStyle())
            {
                dialogueBox_image.sprite = dialogue.GetDialogStyle().GetImage();
                dialogueBox_image.color = dialogue.GetDialogStyle().GetColor();
            }
            else
            {
                dialogueBox_image.sprite = defaultStyle.GetImage();
                dialogueBox_image.color = defaultStyle.GetColor();
            }

            //call run method in displayDialogue, passing in each dialogue in the dialogue object
            yield return displayDialogue.Run(dialogue.GetText(), textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }
        */
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
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedDialogueId = 1;
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
    
}
