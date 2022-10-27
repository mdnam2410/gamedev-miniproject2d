using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndGameView : BaseView
{
    public static readonly string Path = "Prefabs/UI/EndGame";
    public static GameManager.GameEndType GameEndType;

    [SerializeField] GameObject textYouWin;
    [SerializeField] GameObject textYouLose;
    [SerializeField] GameObject textPlayer1Win;
    [SerializeField] GameObject textPlayer2Win;

    [SerializeField] GameObject crown;

    public void Start()
    {
        var texts = new GameObject[] { textYouWin, textYouLose, textPlayer1Win, textPlayer2Win };
        for (int i = 0; i < texts.Length; ++i)
        {
            var text = texts[i];
            if (text != null)
            {
                text.SetActive(i+1 == (int) GameEndType);
            }
        }

        crown.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f).SetDelay(1f).SetEase(Ease.OutQuad);
    }

    public void OnClick_Close()
    {
        SceneManager.UnloadSceneAsync(InGameView.CurrentScene);
        ViewManager.Instance.ChangeMain(MainMenuView.Path);
        ViewManager.Instance.PopTop();
    }

    public void OnClick_BackToMenu()
    {
        SceneManager.UnloadSceneAsync(InGameView.CurrentScene);
        ViewManager.Instance.PopTop();
        ViewManager.Instance.ChangeMain(MainMenuView.Path);
        Time.timeScale = 1f;
    }
}
