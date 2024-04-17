using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public Sounds[] musicSounds, SFXSounds;
    public AudioSource musicSource, SFXSource;

    Scene scene;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0)
            PlayMusic("BGM");
        else if (scene.buildIndex == 1)
            PlayAmbience("Ambience");
    }

    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
            Debug.Log("Audio Not Found");
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sounds s = Array.Find(SFXSounds, x => x.name == name);
        if (s == null)
            Debug.Log("Audio Not Found");
        else
            SFXSource.PlayOneShot(s.clip);
    }

    public void ToggleMusic() => musicSource.mute = !musicSource.mute;

    public void ToggleSFX() => SFXSource.mute = !SFXSource.mute;

    public void MusicVolume(float volume) => musicSource.volume = volume;

    public void SFXVolume(float volume) => SFXSource.volume = volume;

    public void PlayAmbience(string name)
    {
        Sounds s = Array.Find(SFXSounds, x => x.name == name);
        if (s == null)
            Debug.Log("Audio Not Found");
        else
        {
            SFXSource.clip = s.clip;
            SFXSource.Play();
            SFXSource.loop = true;
        }
    }

}
