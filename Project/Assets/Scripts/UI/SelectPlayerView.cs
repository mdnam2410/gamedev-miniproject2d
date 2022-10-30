using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlayerView : BaseView
{
    public static readonly string Path = "Prefabs/UI/SelectPlayer";

    private string sceneToPlay;



    public void Init(string defaultScene)
    {
        sceneToPlay = defaultScene;

    }

    public void OnClick_Start()
    {
        if (!string.IsNullOrEmpty(sceneToPlay))
        {
            ViewManager.Instance.ChangeMain(InGameView.Path);
            SceneManager.LoadScene(sceneToPlay, LoadSceneMode.Additive);
        }
    }
}
