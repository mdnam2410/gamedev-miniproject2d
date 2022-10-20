using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettingsView : MonoBehaviour
{
    public static readonly string Path = "Prefabs/UI/PopupSettings";

    public void OnClick_Close() => ViewManager.Instance.PopTop();
}
