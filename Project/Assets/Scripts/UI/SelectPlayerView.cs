using System.Collections;
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

    [Header("Player A")]
    [SerializeField] Transform avatarContainer1;

    [Header("Player B")]
    [SerializeField] GameObject selectPlayerB;
    [SerializeField] Transform avatarContainer2;


    private string sceneToPlay;
    private GameManager.GameType selectedGameType;
    private string playerASelectedAvatar;
    private string playerBSelectedAvatar;

    private List<SelectPlayerView_AvatarItem> playerAAvatars = new List<SelectPlayerView_AvatarItem>();
    private List<SelectPlayerView_AvatarItem> playerBAvatars = new List<SelectPlayerView_AvatarItem>();

    public void Init(GameManager.GameType gameType)
    {
        sceneToPlay = GameDefine.DEFAULT_SCENE;
        selectedGameType = gameType;

        textMode.text = gameType == GameManager.GameType.vsBot ? "Mode: Vs BOT" : "Mode: Vs Player";
        selectPlayerB.SetActive(gameType == GameManager.GameType.vsPlayer);

        InitSelectPlayerA();
        InitSelectPlayerB();
    }

    public void InitSelectPlayerA()
    {
        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer1, listToUpdate: playerAAvatars);
        }
    }

    public void InitSelectPlayerB()
    {
        if (selectedGameType == GameManager.GameType.vsBot)
            return;

        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer2, listToUpdate: playerBAvatars);
        }
    }

    private SelectPlayerView_AvatarItem GenerateAvatar(ConfigAvatarData config, Transform parent, List<SelectPlayerView_AvatarItem> listToUpdate)
    {
        var avatarItem = Instantiate(avatarItemTemplate, parent);
        avatarItem.Init(config);
        listToUpdate.Add(avatarItem);
        return avatarItem;
    }

    #region Events
    public void OnClick_Close() => ViewManager.Instance.PopTop();

    public void OnClick_Start()
    {
        if (string.IsNullOrEmpty(sceneToPlay))
            return;

        GameConfig.Instance.GameType = selectedGameType;
        switch (selectedGameType)
        {
            case GameManager.GameType.vsBot:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.Avatar = playerASelectedAvatar;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.Bot;
                GameConfig.Instance.PlayerB.Avatar = GameDefine.BOT_AVATAR_PREFAB_PATH;
                break;

            case GameManager.GameType.vsPlayer:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.Avatar = playerASelectedAvatar;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.P2;
                GameConfig.Instance.PlayerB.Avatar = playerBSelectedAvatar;
                break;
        }

        InGameView.CurrentScene = GameDefine.DEFAULT_SCENE;
        ViewManager.Instance.PopTop();
        ViewManager.Instance.ChangeMain(InGameView.Path);
        SceneManager.LoadScene(sceneToPlay, LoadSceneMode.Additive);
    }
    #endregion
}
