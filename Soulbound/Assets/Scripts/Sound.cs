using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class Sound
{
    [Header("Define Clip")]
    public string name;

    public AudioClip clip;

    public AudioClip[] clips;


    [Space(3)]
    [Header("Clip Attributes")]
    [Space(5)]
    public bool loop = false;
    public bool autoplay = false;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    public bool randomizePitch = false;
    public float minPitch;
    public float maxPitch;

    [HideInInspector]
    public AudioSource source;

}
