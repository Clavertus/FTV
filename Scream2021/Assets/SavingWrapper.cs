using FTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    public static SavingWrapper instance;

    const string defaultSaveFile = "save"; 

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            yield return null;
        }

        DontDestroyOnLoad(gameObject);

        //FindObjectOfType<LevelLoader>().CheckpointFadingIn();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.L))
        {
            Load();
        }
        
        if (Input.GetKey(KeyCode.K))
        {
            Save();
        }
    }

    public void CheckpointSave()
    {
        Save();
    }

    public void CheckpointLoad()
    {
        Load();
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
