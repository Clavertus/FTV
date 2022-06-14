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

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            yield return null;
        }

        if (QuicksaveCanvas.instance)
        {
            //QuicksaveCanvas.instance.gameObject.SetActive(false);
        }

        //FindObjectOfType<LevelLoader>().CheckpointFadingIn();
        if(FindObjectOfType<TitleSavingWrapper>() == null)
        {
            //consider to leave this here to easier test levels in Editor
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        }
    }

    public IEnumerator LoadLastScene() 
    {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKey(KeyCode.L))
        {
            Load();
        }
        
        if (Input.GetKey(KeyCode.K))
        {
            Save();
        }*/
    }

    public void CheckpointSave()
    {
        if (QuicksaveCanvas.instance)
        {
            //QuicksaveCanvas.instance.gameObject.SetActive(true);
            QuicksaveCanvas.instance.StartAnimation();
        }

        Save();
    }

    public void CheckpointLoad()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
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
