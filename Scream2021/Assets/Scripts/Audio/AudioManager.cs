using Unity.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        //checking if there is more than one AudioManager in the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);//Not destroying it between scenes so the sound is persistened between transitions

        //assigning the properties of the values of our Sound objects to the actual audio sources
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.volume == 0)
            {
                s.source.volume = 1;
            }
            else
            {
                s.source.volume = s.volume;
            }
            if (s.pitch != 1)
            {
                s.source.pitch = 1;
            }
            else
            {
                s.source.pitch = s.pitch;
            }

            s.source.loop = s.loop;

            if (!s.audioMixerGroup)
            {
                s.source.outputAudioMixerGroup = audioMixerGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = s.audioMixerGroup;
            }
            s.source.playOnAwake = s.playOnAwake;
            if (s.advancedMode)
            {
                s.source.spatialBlend = s.spatialBlend;
                s.source.panStereo = s.stereoPan;
                s.source.rolloffMode = s.rolloffMode;
                s.source.minDistance = s.min;
                s.source.maxDistance = s.max;
            }

            //It should always be at the end after setting all the parameters
            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    //starting to play the sound that has the given name (we should give that name with the enum created after saving our changes with the Save button in Inspector)
    public void PlayFromAudioManager(soundsEnum name)
    {
        Sound s = FindSound(name);
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayFromGameObject(AudioSource audioSource)
    {
        Debug.Log(audioSource.gameObject.name);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public AudioSource AddAudioSourceWithSound(GameObject otherGameObject, soundsEnum soundName)//Should always be called in the Awake method of the otherGameObject
    {
        //Adding a new AudioSource to the given gameObject and assigning it the same values possessed by s.source
        AudioSource s = FindSound(soundName).source;
        AudioSource audioSource = otherGameObject.AddComponent<AudioSource>();

        audioSource.clip = s.clip;
        audioSource.outputAudioMixerGroup = s.outputAudioMixerGroup;
        audioSource.pitch = s.pitch;
        audioSource.loop = s.loop;
        audioSource.spatialBlend = s.spatialBlend;
        audioSource.panStereo = s.panStereo;
        audioSource.rolloffMode = s.rolloffMode;
        audioSource.minDistance = s.minDistance;
        audioSource.maxDistance = s.maxDistance;
        audioSource.playOnAwake = s.playOnAwake;

        if (audioSource.playOnAwake)
        {
            audioSource.Play();
        }

        return audioSource;
    }

    private Sound FindSound(soundsEnum name)
    {
        return Array.Find(sounds, sound => sound.name == name.ToString());
    }
}