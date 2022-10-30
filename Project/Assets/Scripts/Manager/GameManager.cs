using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public float totalTimeOfTurn = 31;
    public float remainingTimeOfTurn;
    public float validTimeForMoving = 15;
    public float validTimeForShooting = 0;
    
    public int windForceScaleFactor = 10;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
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

    public enum GameEndType
    {
        None,
        PlayerWin,
        BotWin,
        Player1Win,
        Player2Win
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
    public CameraController cameraController;

    // Environment - wind
    public float windSpeed;
    public Text windSpeedUI;
    public float timeRandomChangeWindSpeed;
    public float cachedTimeRandomChangeWindSpeed;

    public UnityEvent OnTurnChanged;
    public UnityEvent OnGameEnd = new UnityEvent();
    public UnityEvent OnPlayerShoot;
    public UnityEvent OnBulletDestroyed;

    public float waitingTime = 4f;

    private float dt;

    private void Start()
    {
        this.InitPlayers();
        this.InitEnvironment();
        //this.currentTurn = GameTurn.P1;
        //this.currentPlayer = this.P1;
        this.windSpeed = 0;
        if (this.windSpeedUI != null)
            this.windSpeedUI.text = "0";

        this.OnTurnChanged.AddListener(this.ResetTurnValues);
        this.OnBulletDestroyed.AddListener(this.BulletDestroy);
    }

    public void InitPlayers()
    {
        // TODO
        P1.SetHealthBar(healthBar1);
        P2.SetHealthBar(healthBar2);

        P1.gameObject.transform.position = GetPlayerPos(SpawningPlace.Instance.listPlace1);
        P2.gameObject.transform.position = GetPlayerPos(SpawningPlace.Instance.listPlace2);

        //cameraController.FocusAtPos(P1.transform.position);
    }

    Vector3 GetPlayerPos(List<Transform>listPos)
    {
        int index = Random.Range(0, listPos.Count);
        return listPos[index].position;
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
        //this.UpdateHpUI();
    }

    public void CheckTimeout()
    {
        this.remainingTimeOfTurn -= this.dt;

        if (this.remainingTimeOfTurn <= 0)
        {
            this.remainingTimeOfTurn = 0;
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
        GameEndType gameEndType;
        if (player == P1)
        {
            if (this.gameType == GameType.vsPlayer)
            {
                gameEndType = GameEndType.Player2Win;
                Debug.Log("P2 Win");
            }
            else
            {
                gameEndType = GameEndType.BotWin;
                Debug.Log("YOU LOSE");
            }
        }
        else
        {
            if (this.gameType == GameType.vsPlayer)
            {
                gameEndType = GameEndType.Player1Win;
                Debug.Log("P1 Win");
            }
            else
            {
                gameEndType = GameEndType.PlayerWin;
                Debug.Log("YOU WIN");
            }
        }

        Time.timeScale = 0f;
        EndGameView.GameEndType = gameEndType;
        ViewManager.Instance.PushTop(EndGameView.Path);
    }
}
