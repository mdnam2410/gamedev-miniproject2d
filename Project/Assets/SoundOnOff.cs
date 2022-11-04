using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOff : MonoBehaviour
{
    public void SoundOn()
    {
        if (SoundManager.Instance.Enabled)
            return;

        SoundManager.Instance.SoundOn();
        SoundManager.Instance.PlayMainSound();
    }
    public void SoundOff()
    {
        if (!SoundManager.Instance.Enabled)
            return;
        SoundManager.Instance.SoundOff();
    }
}
