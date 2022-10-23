using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupPauseView : MonoBehaviour
{
    public static readonly string Path = "Prefabs/UI/PopupPause";

    public void OnClick_Close() => OnClick_Resume();

    public void OnClick_Resume()
    {
        ViewManager.Instance.PopTop();
        Time.timeScale = 1f;
    }

    public void OnClick_BackToMenu()
    {
        SceneManager.UnloadSceneAsync(InGameView.CurrentScene);
        ViewManager.Instance.PopTop();
        ViewManager.Instance.ChangeMain(MainMenuView.Path);
        Time.timeScale = 1f;
    }
}
