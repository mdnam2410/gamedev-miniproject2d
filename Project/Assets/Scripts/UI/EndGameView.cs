using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameView : BaseView
{
    public static readonly string Path = "Prefabs/UI/EndGame";

    public void OnClick_Close()
    {
        SceneManager.UnloadSceneAsync(InGameView.CurrentScene);
        ViewManager.Instance.ChangeMain(MainMenuView.Path);
        ViewManager.Instance.PopTop();
    }
}
