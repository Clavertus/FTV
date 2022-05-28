using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDialogue : MonoBehaviour
{
    [SerializeField] float typewriteSpeed = 50f;
    float originalTSpeed; 
    bool typewriting = false;

    private void Start()
    {
        originalTSpeed = typewriteSpeed; 
    }
    private void Update()
    {
        if(typewriting && Input.GetKeyDown(KeyCode.E) && typewriteSpeed == originalTSpeed) 
        {
            typewriteSpeed *= 4;
             
        }
        if (typewriting == false)
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
        int charIndex = 0;
        typewriting = true;
        while(charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriteSpeed; 
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex); 

            yield return null; 
        }
        typewriting = false;
        


        textLabel.text = textToType;
    }

    
}
