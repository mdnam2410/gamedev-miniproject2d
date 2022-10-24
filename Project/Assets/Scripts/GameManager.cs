using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int windForceScaleFactor = 10;
    public float validTimeForMoving = 30;
    public float validTimeForShooting = 0;
    public float remainingTimeOfTurn;

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

    private float dt;

    private void Start()
    {
        this.InitPlayers();
        this.InitEnvironment();
        this.currentTurn = GameTurn.P1;
        this.windSpeed = 0;
        if (this.windSpeedUI != null)
            this.windSpeedUI.text = "0";

        this.OnTurnChanged.AddListener(this.ResetTurnValues);

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
        this.remainingTimeOfTurn -= this.dt;

        this.UpdateTurnUI();
        this.UpdateTurn();
        this.UpdateValidAction();
        this.UpdateEnvironment();
        this.UpdateHpUI();
    }

    public void UpdateTurn()
    {
        if (this.endTurn || this.timeout)
        {
            this.ChangeCurrentTurn();
            this.OnTurnChanged.Invoke();
        }

    }

    public void ResetTurnValues()
    {
        // TODO
        // reset endturn
        // reset timeout
        this.timeout = false;

        // reset remaining time
        this.remainingTimeOfTurn = 45f;

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

    public void EndTurn()
    {
        // called from Bullet
        // called by timeout
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

        Time.timeScale = 0;
    }
}
