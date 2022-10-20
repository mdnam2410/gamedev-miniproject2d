using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHowToPlayView : BaseView
{
    public static readonly string Path = "Prefabs/UI/PopupHowToPlay";

    public void OnClick_Close()
    {
        ViewManager.Instance.PopTop();
    }
}
