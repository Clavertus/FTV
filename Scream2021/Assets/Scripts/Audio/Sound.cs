using System;
using Unity.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool playOnAwake;
    [Header("Advanced Settings")]
    public bool advancedMode;

    [Range(0f, 3f)]
    [Header("                                 2D ---------------------- 3D")]
    public float spatialBlend;

    [Range(-1f, 1f)]
    [Header("                                 Left ----------------- Right")]
    public float stereoPan;

    public AudioSourceCurveType sourceCurveType;
    public AudioRolloffMode rolloffMode;

    [Header("Distance")]
    [Range(0f, 1999f)]
    public float min;
    [Range(5f, 2000f)]
    public float max;

    [HideInInspector]
    public AudioSource source;
}
