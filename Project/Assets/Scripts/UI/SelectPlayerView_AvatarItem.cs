using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerView_AvatarItem : MonoBehaviour
{
    [SerializeField] Image avatarImage;
    [SerializeField] GameObject selected;

    private ConfigAvatarData config;

    public void Init(ConfigAvatarData config)
    {
        this.config = config;
        Select(false);
    }

    public void Select(bool value)
    {
        selected.SetActive(value);
    }
}
