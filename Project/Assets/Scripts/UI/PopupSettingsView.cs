using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupSettingsView : MonoBehaviour
{
    public static readonly string Path = "Prefabs/UI/PopupSettings";

    [SerializeField] TMP_Text textVersion;

    public void Start()
    {
        textVersion.text = Application.version;
    }

    public void OnClick_Close() => ViewManager.Instance.PopTop();
}
