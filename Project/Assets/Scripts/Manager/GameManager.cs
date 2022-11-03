using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [Header("PLAYERS")]
    public PlayerInfo PlayerInfo1;
    public PlayerInfo PlayerInfo2;
    public Player P1;
    public Player P2;
    public Bot P2Bot;
    public Player currentPlayer;

    [Header("TIME AND ENVIRONMENT")]
    public float totalTimeOfTurn = 31;
    public float remainingTimeOfTurn;
    public float validTimeForMoving = 15;
    public float validTimeForShooting = 0;
    public float waitingTime = 4f;
    public float windSpeed;
    public Text windSpeedUI;
    public float timeRandomChangeWindSpeed;
    public float cachedTimeRandomChangeWindSpeed;
    public float windSpeedOfLastShot;
    public int windForceScaleFactor = 10;
    public ConfigAvatar configAvatar;
    public AudioSource mapSound;

    public int timeCount;
    public GameType gameType;
    public GameTurn currentTurn;
    public ValidAction currentValidAction;

    public bool endTurn;
    public bool timeout;

    [Header("UI")]
    public HealthBar healthBar1;
    public HealthBar healthBar2;
    public AngleRuler angleRuler;
    public CameraController cameraController;
    public PlayerIndicator playerIndicator;
    public DamageIndicator damageIndicator;

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



    public UnityEvent OnTurnChanged;
    public UnityEvent OnGameEnd = new UnityEvent();
    public UnityEvent OnPlayerShoot;
    public UnityEvent OnBulletDestroyed;

    

    private float dt;


    private void Start()
    {
        this.LoadGameConfigData();
        this.InitPlayers();
        this.InitEnvironment();
        this.currentTurn = GameTurn.P1;
        this.currentPlayer = this.P1;


        this.OnTurnChanged.AddListener(this.ResetTurnValues);
        this.OnBulletDestroyed.AddListener(this.BulletDestroy);

        this.cameraController.FocusPlayer(P1.gameObject);

        /*
        this.P1.OnBehit.AddListener(damageIndicator.OnBehitCallback);
        this.P2.OnBehit.AddListener(damageIndicator.OnBehitCallback);
        */

    }

    private void LoadGameConfigData()
    {
        // modify later
        int selection = 1;

        if (selection != 0 )
        {
            GameStartData.Instance.gameType = GameConfig.Instance.GameType;
            GameStartData.Instance.P1 = this.configAvatar.GetFromId(GameConfig.Instance.PlayerA.AvatarId);
            GameStartData.Instance.P2 = this.configAvatar.GetFromId(GameConfig.Instance.PlayerB.AvatarId);
            GameStartData.Instance.MapName = GameConfig.Instance.MapName;
        }
        else
        {
            GameStartData.Instance.P1 = configAvatar.Data[0];
            GameStartData.Instance.P2 = configAvatar.Data[1];
            GameStartData.Instance.gameType = GameType.vsPlayer;
            GameStartData.Instance.MapName = "";
        }


        // test only
        GameStartData.Instance.gameType = GameType.vsBot;
    }

    public void InitPlayers()
    {
        this.gameType = GameStartData.Instance.gameType;
        this.currentTurn = GameTurn.None;

        Transform tp1 = this.GetPlayerTransform(SpawningPlace.Instance.listPlace1);
        Transform tp2 = this.GetPlayerTransform(SpawningPlace.Instance.listPlace2);
        GameObject obj1 = Instantiate(GameStartData.Instance.P1.Prefab, tp1);
        GameObject obj2 = Instantiate(GameStartData.Instance.P2.Prefab, tp2);

        this.P1 = EnableExactPlayerComponent(obj1);
        PlayerRef[] playerRefArr1 = this.P1.GetComponentsInChildren<PlayerRef>();
        for (int i = 0; i < playerRefArr1.Length; i++)
        {
            playerRefArr1[i].owner = this.P1;
        }

        if (this.gameType == GameType.vsPlayer)
            this.P2 = EnableExactPlayerComponent(obj2);
        else
            this.P2 = EnableExactBotComponent(obj2);
        PlayerRef[] playerRefArr2 = this.P2.GetComponentsInChildren<PlayerRef>();
        for (int i = 0; i < playerRefArr2.Length; i++)
        {
            playerRefArr2[i].owner = this.P2Bot;
        }
        this.P2Bot.bullet.owner = this.P2Bot;

        this.P1.gameObject.transform.position = tp1.position;
        this.P2.gameObject.transform.position = tp2.position;

        this.P1.healthBar = this.healthBar1;
        this.P2.healthBar = this.healthBar2;

        this.P1.ownRole = GameTurn.P1;
        this.P2.ownRole = GameTurn.P2;
        if (this.gameType == GameType.vsBot) this.P2.ownRole = GameTurn.Bot;
        this.P1.faceDirection = Player.FaceDirection.LeftRight;
        this.P1.transform.localScale = new Vector3(1f, this.P1.transform.localScale.y, this.P1.transform.localScale.z);
        this.P2.faceDirection = Player.FaceDirection.RightLeft;
        this.P2.transform.localScale = new Vector3(-1f, this.P2.transform.localScale.y, this.P2.transform.localScale.z);
        this.P1.target = P2;
        this.P2.target = P1;
        this.currentPlayer = this.P1;
        this.currentTurn = GameTurn.P1;
    }


    private Player EnableExactPlayerComponent(GameObject gameObject)
    {
        int index = 0;
        Player[] playerArr = gameObject.GetComponents<Player>();
        for (int i = 0; i < playerArr.Length; i++)
        {
            Player player = playerArr[i];

            if (player is Bot)
            {
                player.enabled = false;
            }
            else
            {
                player.enabled = true;
                index = i;
            }

        }
        return playerArr[index];
    }

    private Player EnableExactBotComponent(GameObject gameObject)
    {
        int index = 0;
        Player[] playerArr = gameObject.GetComponents<Player>();
        for (int i = 0; i < playerArr.Length; i++)
        {
            Player player = playerArr[i];

            if (player is Bot bot)
            {
                this.P2Bot = bot;
                player.enabled = true;
                index = i;
            }
            else
            {
                player.enabled = false;
            }

        }
        return playerArr[index];
    }
    Transform GetPlayerTransform(List<Transform> listTransform)
    {
        int index = Random.Range(0, listTransform.Count);
        return listTransform[index];
    }

    Vector3 GetPlayerPos(List<Transform> listPos)
    {
        int index = Random.Range(0, listPos.Count);
        return listPos[index].position;
    }

    public void InitEnvironment()
    {
        this.windSpeed = 0;
        if (this.windSpeedUI != null)
            this.windSpeedUI.text = "0";
        this.totalTimeOfTurn = 61;
        this.remainingTimeOfTurn = 61;
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
        this.UpdateBackgroundSound();
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

        this.cameraController.FocusPlayer(this.currentPlayer.gameObject);
    }

    public void UpdateTurnUI()
    {
        if (this.currentTurn == GameTurn.P1)
        {
            this.SetPositionForAngleRuler(P1);
            this.SetPlayerIndicator(P1);
        }
        else
        {
            this.SetPositionForAngleRuler(P2);
            this.SetPlayerIndicator(P2);
        }

    }

    public void SetPositionForAngleRuler(Player player)
    {
        this.angleRuler.transform.position = player.firePos.position;

        Vector3 scale = this.angleRuler.transform.localScale;
        float xScale = player.transform.localScale.x;
        this.angleRuler.transform.localScale = new Vector3(-Mathf.Abs(scale.x) * xScale / Mathf.Abs(xScale), scale.y, scale.z);
    }

    public void SetPlayerIndicator(Player player)
    {
        // Position
        var headPosition = player.heroHead.transform.position;
        this.playerIndicator.transform.position = headPosition + Vector3.up * 0.5f;

        // Name
        this.playerIndicator.SetName(GameDefine.GetPlayerDisplayName(this.gameType, this.currentTurn));
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

    public void SaveWindSpeed() => this.windSpeedOfLastShot = this.windSpeed;

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

    public void UpdateBackgroundSound()
    {
        if (!this.mapSound.isPlaying)
        {
            this.mapSound.Stop();
            this.mapSound.Play();
        }
    }
}
