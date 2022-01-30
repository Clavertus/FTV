using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

public class DiscordManager : MonoBehaviour
{
    public Discord.Discord discord;
    public ActivityManager activityManager;

    public static DiscordManager instance;

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

        try
        {
            discord = new Discord.Discord(908621606396522510, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
            activityManager = discord.GetActivityManager();
            try
            {
                Activity activity = new Activity
                {
                    State = "The journey begins",
                    Assets =
                    {
                        LargeImage = "icon",
                    },
                    Timestamps =
                    {
                        Start = 0
                    }
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res == Result.Ok)
                    {
                        Debug.Log("Discord status set!");
                    }
                    else
                    {
                        Debug.LogError("Discord status failed!");
                    }
                });
            }
            catch (System.Exception)
            {
                OnDisable();
            }
        }
        catch (System.Exception)
        {
            OnDisable();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            UpdateActivity("The journey begins");
        }
        else if (SceneManager.GetActiveScene().name == "Train Station")
        {
            UpdateActivity("On the train platform");
        }
        else if (SceneManager.GetActiveScene().name == "PreVoid")
        {
            UpdateActivity("Commuting...");
        }
        else if (SceneManager.GetActiveScene().name == "Inside Train")
        {
            UpdateActivity("Into the Void");
        }
    }

    void Update()
    {
        discord?.RunCallbacks();
    }

    void UpdateActivity(string state)
    {
        try
        {
            Activity activity = new Activity
            {
                State = state,
                Assets =
                {
                    LargeImage = "icon",
                },
                Timestamps =
                    {
                        Start = 0
                    }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Result.Ok)
                {
                    Debug.Log("Discord status set!");
                }
                else
                {
                    Debug.LogError("Discord status failed!");
                }
            });
        }
        catch (System.Exception)
        {
            OnDisable();
        }
    }

    void OnDisable()
    {
        Debug.Log("Deleting!");
        discord?.Dispose();
    }
}
