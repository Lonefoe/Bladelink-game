using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

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

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    private void Start()
    {
        Play("MainTheme");
        Play("Wind");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        if (s.clip != null)
        {
            s.source.Play();
        } else if (s.clips != null)
        {
            int num = UnityEngine.Random.Range(1, s.clips.Length);

            s.source.clip = s.clips[num];
            s.source.Play();
        }
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        if (s.clip != null)
        {
            s.source.PlayOneShot(s.clip);
        } else if (s.clips != null)
        {
            int num = UnityEngine.Random.Range(1, s.clips.Length);

            s.source.PlayOneShot(s.clips[num]);
        }

    }

}