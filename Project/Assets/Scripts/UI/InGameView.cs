using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameView : BaseView
{
    public static readonly string Path = "Prefabs/UI/InGame";
    public static string CurrentScene;

    public void OnClick_Pause()
    {
        Time.timeScale = 0f;
        ViewManager.Instance.PushTop(PopupPauseView.Path);
    }
}
