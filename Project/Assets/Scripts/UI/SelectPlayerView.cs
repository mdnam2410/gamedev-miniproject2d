using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectPlayerView : BaseView
{
    public static readonly string Path = "Prefabs/UI/SelectPlayer";

    [Header("Templates")]
    [SerializeField] SelectPlayerView_AvatarItem avatarItemTemplate;

    [Header("Texts")]
    [SerializeField] TMP_Text textMode;
    [SerializeField] GameObject textDescriptionPlayerA;
    [SerializeField] GameObject textDescriptionPlayerB;

    [Header("Player A")]
    [SerializeField] Transform avatarContainer1;

    [Header("Player B")]
    [SerializeField] GameObject selectPlayerB;
    [SerializeField] Transform avatarContainer2;


    private string sceneToPlay;
    private GameManager.GameType selectedGameType;
    private int playerASelectedAvatar;
    private int playerBSelectedAvatar;

    private List<SelectPlayerView_AvatarItem> playerAAvatars = new List<SelectPlayerView_AvatarItem>();
    private List<SelectPlayerView_AvatarItem> playerBAvatars = new List<SelectPlayerView_AvatarItem>();

    public void Init(GameManager.GameType gameType)
    {
        sceneToPlay = GameDefine.DEFAULT_SCENE;
        selectedGameType = gameType;

        textMode.text = gameType == GameManager.GameType.vsBot ? "Mode: Vs BOT" : "Mode: Vs Player";
        textDescriptionPlayerA.SetActive(gameType == GameManager.GameType.vsPlayer);
        textDescriptionPlayerB.SetActive(gameType == GameManager.GameType.vsPlayer);
        selectPlayerB.SetActive(gameType == GameManager.GameType.vsPlayer);

        InitSelectPlayerA();
        InitSelectPlayerB();
    }

    #region Initializing
    public void InitSelectPlayerA()
    {
        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer1, listToUpdate: playerAAvatars, onClickCallback: AvatarItemPlayerAOnClickCallback);
        }
        playerASelectedAvatar = playerAAvatars[0].AvatarId;
        playerAAvatars[0].OnClick();
    }

    public void InitSelectPlayerB()
    {
        if (selectedGameType == GameManager.GameType.vsBot)
        {
            playerBSelectedAvatar = GameDefine.BOT_AVATAR_ID;
            return;
        }

        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer2, listToUpdate: playerBAvatars, onClickCallback: AvatarItemPlayerBOnClickCallback);
        }
        playerBSelectedAvatar = playerBAvatars[0].AvatarId;
        playerBAvatars[0].OnClick();
    }

    private SelectPlayerView_AvatarItem GenerateAvatar(ConfigAvatarData config, Transform parent, List<SelectPlayerView_AvatarItem> listToUpdate, System.Action<int> onClickCallback)
    {
        var avatarItem = Instantiate(avatarItemTemplate, parent);
        avatarItem.Init(config, onClickCallback);
        listToUpdate.Add(avatarItem);
        return avatarItem;
    }
    #endregion

    #region Interactions
    void AvatarItemPlayerAOnClickCallback(int selectedAvatarId)
    {
        playerAAvatars.RemoveAll(x => x == null);
        playerASelectedAvatar = selectedAvatarId;
        for (int i = 0; i < playerAAvatars.Count; ++i)
        {
            playerAAvatars[i].Select(playerAAvatars[i].AvatarId == selectedAvatarId);
        }
    }

    void AvatarItemPlayerBOnClickCallback(int selectedAvatarId)
    {
        playerBAvatars.RemoveAll(x => x == null);
        playerBSelectedAvatar = selectedAvatarId;
        for (int i = 0; i < playerBAvatars.Count; ++i)
        {
            playerBAvatars[i].Select(playerBAvatars[i].AvatarId == selectedAvatarId);
        }
    }

    #endregion

    #region Events
    public void OnClick_Close() => ViewManager.Instance.PopTop();

    public void OnClick_Start()
    {
        if (string.IsNullOrEmpty(sceneToPlay))
            return;

        // Set game configuration
        GameConfig.Instance.MapName = GameDefine.DEFAULT_SCENE;
        GameConfig.Instance.GameType = selectedGameType;
        switch (selectedGameType)
        {
            case GameManager.GameType.vsBot:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.AvatarId = playerASelectedAvatar;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.Bot;
                GameConfig.Instance.PlayerB.AvatarId = GameDefine.BOT_AVATAR_ID;
                break;

            case GameManager.GameType.vsPlayer:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.AvatarId = playerASelectedAvatar;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.P2;
                GameConfig.Instance.PlayerB.AvatarId = playerBSelectedAvatar;
                break;
        }
        GameConfig.Instance.PrintDebugInfo();

        // Change view
        InGameView.CurrentScene = GameConfig.Instance.MapName;
        ViewManager.Instance.PopTop();
        ViewManager.Instance.ChangeMain(InGameView.Path);
        SceneManager.LoadScene(sceneToPlay, LoadSceneMode.Additive);
    }
    #endregion
}
