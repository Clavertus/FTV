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
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }

    private void Save()
    {
        //call to saving system save
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    private void Load()
    {
        //call to saving system load
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
}
