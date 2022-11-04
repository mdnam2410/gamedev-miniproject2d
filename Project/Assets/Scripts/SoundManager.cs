using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private static SoundManager instance;
    private SoundManager() { }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = new SoundManager();
            return instance;
        }
    }

    private bool soundEnabled = true;
    private List<AudioSource> soundList = new List<AudioSource>();
    private AudioSource mainSound;

    public bool Enabled
    {
        get => soundEnabled;
    }

    public void SoundOn() => soundEnabled = true;
    public void SoundOff()
    {
        soundEnabled = false;
        foreach (AudioSource audio in soundList)
        {
            if (audio.isActiveAndEnabled)
            {
                audio.Stop();
            }
        }
    }

    public void PlaySound(AudioSource audioSource, bool isMainSound)
    {
        if (!Enabled) return;
        if (!audioSource.isActiveAndEnabled) return;

        audioSource.Play();
        if (!soundList.Contains(audioSource))
        {
            soundList.Add(audioSource);
        }

        if (isMainSound)
            mainSound = audioSource;

    }

    public void PlayThisSoundOnly(AudioSource audioSource, bool isMainSound)
    {
        if (!Enabled) return;
        if (!audioSource.isActiveAndEnabled) return;

        foreach (AudioSource other in soundList)
        {
            if (other.isActiveAndEnabled && other != audioSource)
            {
                other.Stop();
            }
        }

        if (!soundList.Contains(audioSource))
        {
            soundList.Add(audioSource);
        }
        audioSource.Play();

        if (isMainSound)
            mainSound = audioSource;
    }

    public void PlayMainSound()
    {
        if (!Enabled) return;

        foreach (AudioSource sound in soundList)
        {
            if (sound.isActiveAndEnabled)
            {
                sound.Stop();
            }
        }

        if (mainSound == null || !mainSound.isActiveAndEnabled)
            return;
        mainSound.Play();
    }

    public void RemoveSound(AudioSource audioSource)
    {
        if (soundList.Contains(audioSource))
        {
            soundList.Remove(audioSource);
        }
    }
}
