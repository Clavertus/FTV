using Unity.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public Sound[] sounds;

    void Awake()
    {
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
            if (s.pitch == 0)
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
            //It should always be at the end after setting all the parameters
            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    //starting to play the sound that has the given name (we should give that name with the enum created after saving our changes with the Save button in Inspector)
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.source.clip);
    }
}
