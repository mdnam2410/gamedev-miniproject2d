using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    public AudioSource mainBGM;
    public Camera isoCamera;

    private void Awake()
    {
        this.mainBGM.Play();
    }

    private void Update()
    {
        if(this.isoCamera != Camera.main)
        {
            this.mainBGM.Stop();
        }
        else
        {
            if (!this.mainBGM.isPlaying)
            {
                this.mainBGM.Play();
            }
        }
    }
}
