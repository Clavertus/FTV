using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save"; 

    public void DeleteSaveFile()
    {
        FindObjectOfType<SavingSystem>().Delete(defaultSaveFile);
    }

    public bool CheckSaveGame()
    {
        //call to saving system to check save file
        return FindObjectOfType<SavingSystem>().checkSave(defaultSaveFile);
    }

    public void LoadLastGame()
    {
        //call to saving system load
        StartCoroutine(SavingWrapper.instance.LoadLastScene());
    }
}
