using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SurfaceManager : MonoBehaviour
{
    public static SurfaceManager Instance { get; private set; }
    private Sound footstepSound;
    public AudioClip[] stoneSounds, grassSounds, woodSounds;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start() {
        footstepSound = Array.Find(AudioManager.Instance.sounds, sound => sound.name == "PlayerFootstep");
    }

    public void UpdateGround(SurfaceType surf)
    {
        switch(surf)
        {
            case SurfaceType.Stone:
            footstepSound.clips = stoneSounds;
            break;

            case SurfaceType.Grass:
            footstepSound.clips = grassSounds;
            break;

            case SurfaceType.Wood:
            footstepSound.clips = woodSounds;
            break;
        }
    }

}
