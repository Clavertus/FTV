using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDialogue : MonoBehaviour
{
    [SerializeField] float typewriteSpeed = 50f;
    [SerializeField] float fixedDialogDisplayTime = 0.35f;
    float originalTSpeed; 
    bool typewriting = false;
    bool allowDialogSkip = false;

    private void Start()
    {
        originalTSpeed = typewriteSpeed; 
    }
    private void Update()
    {
        if(typewriting && Input.GetKeyDown(KeyCode.E) && (typewriteSpeed == originalTSpeed) && (allowDialogSkip == true)) 
        {
            typewriteSpeed *= 4;
        }
        if ((typewriting == false) || (allowDialogSkip == false))
        {
            typewriteSpeed = originalTSpeed; 
        }
    }
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));  
    }
    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        float t = 0;
        float time = 0f;
        int charIndex = 0;
        typewriting = true;
        while(charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriteSpeed;
            time += Time.deltaTime;
            if (time > fixedDialogDisplayTime)
            {
                allowDialogSkip = true;
            }
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex); 

            yield return null; 
        }
        typewriting = false;
        allowDialogSkip = false;


        textLabel.text = textToType;
    }

    
}
