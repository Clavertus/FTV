using FTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour, ISaveable
{
    public static ProgressTracker instance;
    public enum endingType
    {
        Good,
        Bad,
        Unknown
    }

    public endingType taraEnding = endingType.Unknown;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public int taraEndingInt;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.taraEndingInt = (int) taraEnding;

        return data;
    }

    public void RestoreState(object state)
    {
        Debug.LogWarning("Restoring tara state");

        SaveData data = (SaveData)state;
        taraEnding = (endingType) data.taraEndingInt;
        Debug.Log("taraEnding: " + taraEnding);
    }

    #endregion

}
