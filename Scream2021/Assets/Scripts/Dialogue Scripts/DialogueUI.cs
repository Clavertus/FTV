using System.Collections; 
using UnityEngine;
using TMPro; 

public class DialogueUI : MonoBehaviour
{
    [SerializeField] TMP_Text textLabel;
    [SerializeField] DialogueObject testDialogue;
    [SerializeField] GameObject dialogueBox;
    DisplayDialogue displayDialogue; 
    private void Start()
    {
        displayDialogue = GetComponent<DisplayDialogue>();
        CloseDialogueBox();
        //gives the desired dialogue object
        ShowDialogue(testDialogue); 
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);

        FindObjectOfType<PlayerMovement>().LockPlayer(); 
        FindObjectOfType<SelectionManager>().LockSelection();
        
        StartCoroutine(StepThroughDialogue(dialogueObject)); 
    }

    IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        
        foreach(string dialogue in dialogueObject.Dialogue) 
        {
            //call run method in displayDialogue, passing in each dialogue in the dialogue object
            yield return displayDialogue.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));  
        }
        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;

        FindObjectOfType<PlayerMovement>().UnlockPlayer(); 
        FindObjectOfType<SelectionManager>().UnlockSelection();
        FindObjectOfType<Examine>().ExitExamineMode();
    } 
    
}
