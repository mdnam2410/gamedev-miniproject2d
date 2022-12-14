using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
    public static readonly string Path = "Prefabs/UI/MainMenu";

    [Header("Buttons")]
    [SerializeField] GameObject btnNewGame;
    [SerializeField] GameObject btnVsBot;
    [SerializeField] GameObject btnVsPlayer;
    [SerializeField] GameObject btnBack;
    [SerializeField] GameObject btnSetting;

    public void OnClick_NewGame()
    {
        ShowHide(new List<GameObject>() { btnVsBot, btnVsPlayer, btnBack }, true);
        ShowHide(new List<GameObject>() { btnNewGame }, false);
    }

    public void OnClick_Back()
    {
        ShowHide(new List<GameObject>() { btnVsBot, btnVsPlayer, btnBack }, false);
        ShowHide(new List<GameObject>() { btnNewGame }, true);
    }

    public void OnClick_HowToPlay()
    {
        ViewManager.Instance.PushTop(PopupHowToPlayView.Path);
    }

    public void OnClick_Settings()
    {
        ViewManager.Instance.PushTop(PopupSettingsView.Path);
    }

    public void OnClick_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnClick_LoadVsBotMode()
    {
        var view = ViewManager.Instance.PushTop(SelectPlayerView.Path) as SelectPlayerView;
        view.Init(GameManager.GameType.vsBot);
    }

    public void OnClick_LoadVSPlayerMode()
    {
        var view = ViewManager.Instance.PushTop(SelectPlayerView.Path) as SelectPlayerView;
        view.Init(GameManager.GameType.vsPlayer);
    }

    void ShowHide(List<GameObject> gameObjects, bool value)
    {
        foreach(var go in gameObjects)
        {
            go.SetActive(value);
        }
    }
}
