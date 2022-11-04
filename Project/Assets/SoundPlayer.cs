using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource prefab;
    public AudioSource audioSource;
    public PlayingMode playingMode;

    public enum PlayingMode
    {
        Replace,
        Parallel
    }

    public bool isMainSound;

    private void Awake()
    {
        audioSource = Instantiate(prefab, this.transform);
    }

    private void Start()
    {
        if (playingMode == PlayingMode.Parallel)
        {
            SoundManager.Instance.PlaySound(audioSource, isMainSound);
        }

        else
        {
            SoundManager.Instance.PlayThisSoundOnly(audioSource, isMainSound);
        }
    }

    private void OnDestroy()
    {
        SoundManager.Instance.RemoveSound(this.audioSource);
        SoundManager.Instance.PlayMainSound();
    }
}
