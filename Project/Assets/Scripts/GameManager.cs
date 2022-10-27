using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private GameManager() { }

    public float totalTimeOfTurn = 20;
    public float remainingTimeOfTurn;
    public float validTimeForMoving = 10;
    public float validTimeForShooting = 0;
    
    public int windForceScaleFactor = 10;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
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
        Bot
    }

    public enum ValidAction
    {
        None,
        All,
        ShootOnly
    }

    public int timeCount;
    public GameType gameType;
    public GameTurn currentTurn;
    public ValidAction currentValidAction;

    public bool endTurn;
    public bool timeout;

    // Players
    public Player P1;
    public Player P2;
    public Player currentPlayer;

    // UIs
    public HealthBar healthBar1;
    public HealthBar healthBar2;
    public AngleRuler angleRuler;

    // Environment - wind
    public float windSpeed;
    public Text windSpeedUI;
    public float timeRandomChangeWindSpeed;
    public float cachedTimeRandomChangeWindSpeed;

    public UnityEvent OnTurnChanged;
    public UnityEvent OnGameEnd = new UnityEvent();
    public UnityEvent OnPlayerShoot;
    public UnityEvent OnBulletDestroyed;

    public float waitingTime = 2f;

    private float dt;

    private void Start()
    {
        this.InitPlayers();
        this.InitEnvironment();
        this.currentTurn = GameTurn.P1;
        this.currentPlayer = this.P1;
        this.windSpeed = 0;
        if (this.windSpeedUI != null)
            this.windSpeedUI.text = "0";

        this.OnTurnChanged.AddListener(this.ResetTurnValues);
        this.OnBulletDestroyed.AddListener(this.BulletDestroy);
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

        this.CheckTimeout();
        this.UpdateTurnUI();
        this.UpdateTurn();
        this.UpdateValidAction();
        this.UpdateEnvironment();
        this.UpdateHpUI();
    }

    public void CheckTimeout()
    {
        this.remainingTimeOfTurn -= this.dt;

        if (this.remainingTimeOfTurn <= 0)
        {
            if (this.currentPlayer.currentStatus != Player.Status.Attacking)
            {
                this.timeout = true;
            }
            else
            {
                //wait
            }
        }
    }

    public void UpdateTurn()
    {
        if (this.endTurn || this.timeout)
        {
            this.ChangeCurrentTurn();
            this.OnTurnChanged.Invoke();
        }
    }

    public void BulletDestroy()
    {
        this.LockAllPlayerActions();
        Invoke(nameof(this.EndTurn), this.waitingTime);
    }

    public void LockAllPlayerActions() => GameManager.Instance.currentValidAction = ValidAction.None;

    public void EndTurn() => this.endTurn = true;



    public void ResetTurnValues()
    {
        // reset endturn
        this.endTurn = false;
        // reset timeout
        this.timeout = false;

        // reset remaining time
        this.remainingTimeOfTurn = this.totalTimeOfTurn;

        // reset valid action
        this.currentValidAction = ValidAction.All;

    }

    public void UpdateValidAction()
    {
        if (this.remainingTimeOfTurn >= this.validTimeForMoving)
        {
            this.currentValidAction = ValidAction.All;
        }
        else if (this.remainingTimeOfTurn >= this.validTimeForShooting)
        {
            this.currentValidAction = ValidAction.ShootOnly;
        }
        else
        {
            // not reach
            this.currentValidAction = ValidAction.None;
        }
    }

    public void ChangeCurrentTurn()
    {
        if (this.gameType == GameType.vsBot)
        {
            if (this.currentTurn == GameTurn.P1) this.currentTurn = GameTurn.Bot;
            else this.currentTurn = GameTurn.P1;
        }
        else
        {
            if (this.currentTurn == GameTurn.P1) this.currentTurn = GameTurn.P2;
            else this.currentTurn = GameTurn.P1;
        }

        if (this.currentPlayer == this.P1) this.currentPlayer = this.P2;
        else this.currentPlayer = this.P1;
    }

    public void UpdateTurnUI()
    {
        if (this.currentTurn == GameTurn.P1)
        {
            this.SetPositionForAngleRuler(P1);
        }
        else
        {
            this.SetPositionForAngleRuler(P2);
        }
        
    }

    public void SetPositionForAngleRuler(Player player)
    {
        this.angleRuler.transform.position = player.firePos.position;

        Vector3 scale = this.angleRuler.transform.localScale;
        float xScale = player.transform.localScale.x;
        this.angleRuler.transform.localScale = new Vector3(-Mathf.Abs(scale.x) * xScale / Mathf.Abs(xScale), scale.y, scale.z);
    }

    public void UpdateEnvironment()
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

    public void UpdateHpUI()
    {
        healthBar1.SetHealth(P1.hp);
        healthBar2.SetHealth(P2.hp);

    }

    private void DisplayEndGameInfo()
    {
        // TODO
    }

    public void PlayerDefeated(Player player)
    {
        if (player == P1)
        {
            if (this.gameType == GameType.vsPlayer) Debug.Log("P2 Win");
            else Debug.Log("YOU LOSE");
        }
        else
        {
            if (this.gameType == GameType.vsPlayer) Debug.Log("P1 Win");
            else Debug.Log("YOU WIN");
        }

        Time.timeScale = 0f;
        ViewManager.Instance.PushTop(EndGameView.Path);
    }
}
