using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineCanvas : MonoBehaviour
{
    [SerializeField] GameObject extraField = null;
    //this is used as a type
    private void Start()
    {
        extraField.SetActive(false);
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void SetExtraFieldToState(bool state)
    {
        Debug.LogWarning("Setting extra field to " + state);
        extraField.SetActive(state);
    }

    private void OnEnable()
    {
        extraField.SetActive(false);
    }

    private void OnDisable()
    {
        extraField.SetActive(false);
    }
}
