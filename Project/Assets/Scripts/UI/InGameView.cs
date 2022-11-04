using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameView : BaseView
{
    public static readonly string Path = "Prefabs/UI/InGame";
    public static string CurrentScene;

    [Header("Health bar")]
    [SerializeField] HealthBar healthBar1;
    [SerializeField] HealthBar healthBar2;

    [Header("Wind speed")]
    [SerializeField] TMP_Text textWindspeed;
    [SerializeField] GameObject arrowLeft;
    [SerializeField] GameObject arrowRight;

    [Header("Countdown")]
    [SerializeField] TMP_Text textCountdown;

    private void Start()
    {

    }

    private void Update()
    {
        healthBar1.SetHealth(GameManager.Instance.P1.hp);
        healthBar2.SetHealth(GameManager.Instance.P2.hp);

        float wind = GameManager.Instance.windSpeed;
        textWindspeed.text = Mathf.Abs(wind).ToString();
        arrowLeft.SetActive(wind < 0);
        arrowRight.SetActive(wind > 0);

        textCountdown.text = ((int)GameManager.Instance.remainingTimeOfTurn).ToString();
    }

    public void BindPlayerName()
    {
        healthBar1.SetPlayerName(GameDefine.GetPlayerDisplayName(GameConfig.Instance.GameType, GameManager.GameTurn.P1));
        healthBar2.SetPlayerName(GameDefine.GetPlayerDisplayName(GameConfig.Instance.GameType, GameManager.GameTurn.P2));
    }

    public void OnClick_Pause()
    {
        Time.timeScale = 0f;
        ViewManager.Instance.PushTop(PopupPauseView.Path);
    }

    public void OnGameEnd()
    {
        Time.timeScale = 0f;
    }
}
