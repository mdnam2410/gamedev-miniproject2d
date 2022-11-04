using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerView_MapItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text mapName;
    [SerializeField] GameObject selected;
    [SerializeField] Image thumbnail;

    System.Action<string> onClick;
    ConfigMapData config;

    public string MapName { get => config.Name; }

    public void Init(ConfigMapData config, System.Action<string> onClick)
    {
        this.config = config;
        this.onClick = onClick;

        mapName.text = config.Name;
        thumbnail.sprite = config.Sprite;
    }

    public void Select(bool value) => selected.SetActive(value);

    public void OnClick()
    {
        onClick?.Invoke(config.Name);
    }
}
