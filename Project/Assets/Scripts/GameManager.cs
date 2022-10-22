using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int windForceScaleFactor = 10;

    private void Awake()
    {
        instance = this;
    }
    public enum GameType
    {
        vsBot,
        vsPlayer
    }

    public enum GameTurn
    {
        None,
        P1,
        P2,
        Player,
        Bot
    }

    public int timeCount;
    public GameType gameType;
    public GameTurn currentTurn;
    public bool endTurn;
    
    // Players
    public List<Player> playerList;

    // Environment - wind
    public float windSpeed;
    public Text windSpeedUI;
    public float timeRandomChangeWindSpeed;
    public float cachedTimeRandomChangeWindSpeed;
    
    private float dt;

    private void Start()
    {
        this.InitPlayers();
        this.InitEnvironment();
        this.currentTurn = GameTurn.None;
        this.windSpeed = 0;
        if (this.windSpeedUI != null) 
            this.windSpeedUI.text = "0";
    }

    public void InitPlayers()
    {
        // TODO
    }

    public void InitEnvironment()
    {
        // TODO
    }

    public void SetData(GameType gameType, GameTurn gameTurn)
    {
        this.gameType = gameType;
        this.currentTurn = gameTurn;
    }

    private void Update()
    {
        this.dt = Time.deltaTime;

        this.UpdateTurn();
        this.UpdateWindForce();
        this.UpdateGameEnd();
    }

    public void UpdateTurn()
    {
        if (this.endTurn)
        {
            ChangeCurrentTurn();
        }
    }

    public GameTurn ChangeCurrentTurn()
    {
        if (this.gameType == GameType.vsBot)
        {
            if (this.currentTurn == GameTurn.Player) return GameTurn.Bot;
            else return GameTurn.Player;
        }
        else
        {
            if (this.currentTurn == GameTurn.P1) return GameTurn.P2;
            else return GameTurn.P1;
        }
    }

    public void UpdateWindForce()
    {
        this.cachedTimeRandomChangeWindSpeed -= this.dt;
        if (this.cachedTimeRandomChangeWindSpeed < 0)
        {
            this.cachedTimeRandomChangeWindSpeed = this.timeRandomChangeWindSpeed;
            this.timeRandomChangeWindSpeed = UnityEngine.Random.Range(10f, 20f);
            this.RandomChangeWindSpeed();
        }
    }

    public void RandomChangeWindSpeed()
    {
        this.windSpeed = UnityEngine.Random.Range(-10, 11);
        if (this.windSpeedUI != null)
        this.windSpeedUI.text = windSpeed.ToString();
    }

    public void UpdateGameEnd()
    {
        foreach (Player player in this.playerList)
        {
            if (player.hp <= 0)
            {
                Time.timeScale = 0f;
                this.DisplayEndGameInfo();
            }
        }
    }

    private void DisplayEndGameInfo()
    {
        // TODO
    }
}
