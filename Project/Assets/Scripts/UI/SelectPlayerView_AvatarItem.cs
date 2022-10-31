using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerView_AvatarItem : MonoBehaviour
{
    public int AvatarId => config.Id;
    
    [SerializeField] Image avatarImage;
    [SerializeField] GameObject selected;

    private ConfigAvatarData config;
    private Action<int> onClick;


    public void Init(ConfigAvatarData config, Action<int> onClickCallback)
    {
        this.config = config;
        avatarImage.sprite = config.Sprite;
        avatarImage.preserveAspect = true;
        onClick = onClickCallback;
        Select(false);
    }

    public void Select(bool value)
    {
        selected.SetActive(value);
    }

    public void OnClick() => onClick?.Invoke(AvatarId);
}
