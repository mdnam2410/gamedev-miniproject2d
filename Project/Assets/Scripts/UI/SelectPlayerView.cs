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
    [SerializeField] TMP_Text textDescription1;
    [SerializeField] GameObject selectedLine1;
    [SerializeField] TMP_Text textDescription2;
    [SerializeField] GameObject selectedLine2;

    [SerializeField] Transform avatarContainer;

    [Header("Player Stat Preview")]
    [SerializeField]
    SelectPlayerView_PlayerStatPreview playerStatPreview;
    [SerializeField]
    SelectPlayerView_PlayerStatPreview playerStatPreview2;

    [Header("Select map")]
    [SerializeField] SelectPlayerView_MapItem mapItemTemplate;
    [SerializeField] Transform mapItemContainer;

    private string selectedScene;
    private GameManager.GameType selectedGameType;
    private int selectedTab = 0;
    private int selectedAvatar_playerA;
    private int selectedAvatar_playerB;

    private List<SelectPlayerView_AvatarItem> listAvatars = new List<SelectPlayerView_AvatarItem>();
    private List<SelectPlayerView_MapItem> listMaps = new List<SelectPlayerView_MapItem>();

    public void Init(GameManager.GameType gameType)
    {
        selectedScene = GameDefine.DEFAULT_SCENE;
        selectedGameType = gameType;

        textMode.text = gameType == GameManager.GameType.vsBot ? "Mode: Vs BOT" : "Mode: Vs Player";

        if (selectedGameType == GameManager.GameType.vsBot)
            InitModeSinglePlayer();
        else
            InitModeMultiplayer();

        InitMap();
    }

    #region Initializing
    public void InitModeSinglePlayer()
    {
        textDescription2.gameObject.SetActive(false);
        playerStatPreview2.gameObject.SetActive(false);

        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer, listToUpdate: listAvatars, onClickCallback: AvatarOnClickCallback);
        }
        selectedAvatar_playerA = listAvatars[0].AvatarId;
        listAvatars[0].OnClick();
    }

    private void InitModeMultiplayer()
    {
        var configAvatars = ConfigManager.Instance.ConfigAvatar.Data;
        for (int i = 0; i < configAvatars.Length; ++i)
        {
            var config = configAvatars[i];
            GenerateAvatar(config, parent: avatarContainer, listToUpdate: listAvatars, onClickCallback: AvatarOnClickCallback);
        }
        selectedAvatar_playerA = listAvatars[0].AvatarId;
        listAvatars[0].OnClick();

        selectedTab = 1;
        selectedAvatar_playerB = listAvatars[0].AvatarId;
        listAvatars[0].OnClick();

        selectedTab = 0;
    }

    private void InitMap()
    {
        foreach (var config in ConfigManager.Instance.ConfigMap.Data)
        {
            GenerateMap(config, mapItemContainer, listMaps, OnClickCallback_Map);
        }

        selectedScene = listMaps[0].MapName;
        listMaps[0].OnClick();
    }

    private SelectPlayerView_AvatarItem GenerateAvatar(ConfigAvatarData config, Transform parent, List<SelectPlayerView_AvatarItem> listToUpdate, System.Action<int> onClickCallback)
    {
        var avatarItem = Instantiate(avatarItemTemplate, parent);
        avatarItem.Init(config, onClickCallback);
        listToUpdate.Add(avatarItem);
        return avatarItem;
    }

    private SelectPlayerView_MapItem GenerateMap(ConfigMapData config, Transform parent, List<SelectPlayerView_MapItem> listToUpdate, System.Action<string> onClickCallback)
    {
        var item = Instantiate(mapItemTemplate, mapItemContainer);
        item.Init(config, onClickCallback);
        listToUpdate.Add(item);
        return item;
    }
    #endregion

    #region Interactions
    void AvatarOnClickCallback(int selectedAvatarId)
    {
        if (selectedGameType == GameManager.GameType.vsPlayer)
        {
            AvatarOnClickCallback_ModeMultiplayer(selectedAvatarId);
        }
        else
        {
            listAvatars.RemoveAll(x => x == null);
            selectedAvatar_playerA = selectedAvatarId;
            for (int i = 0; i < listAvatars.Count; ++i)
            {
                listAvatars[i].Select(listAvatars[i].AvatarId == selectedAvatarId);
            }

            var config = ConfigManager.Instance.ConfigAvatar.GetFromId(selectedAvatarId);
            playerStatPreview.Init(config);
            playerStatPreview.DisplayPlayerStat();
        }
    }

    void AvatarOnClickCallback_ModeMultiplayer(int selectedAvatar)
    {
        listAvatars.RemoveAll(x => x == null);

        if (selectedTab == 0)
        {
            selectedAvatar_playerA = selectedAvatar;
            for (int i = 0; i < listAvatars.Count; ++i)
            {
                listAvatars[i].Select(listAvatars[i].AvatarId == selectedAvatar, mode: 1);
            }
            var config = ConfigManager.Instance.ConfigAvatar.GetFromId(selectedAvatar);
            playerStatPreview.Init(config, "Player 1");
            playerStatPreview.DisplayPlayerStat();
        }
        else
        {
            selectedAvatar_playerB = selectedAvatar;
            for (int i = 0; i < listAvatars.Count; ++i)
            {
                listAvatars[i].Select(listAvatars[i].AvatarId == selectedAvatar, mode: 2);
            }
            var config = ConfigManager.Instance.ConfigAvatar.GetFromId(selectedAvatar);
            playerStatPreview2.Init(config, "Player 2");
            playerStatPreview2.DisplayPlayerStat();
        }
    }

    public void OnClickCallback_Tab1()
    {
        selectedLine1.SetActive(true);
        selectedLine2.SetActive(false);
        selectedTab = 0;
    }
    public void OnClickCallback_Tab2()
    {
        selectedLine2.SetActive(true);
        selectedLine1.SetActive(false);
        selectedTab = 1;
    }

    public void OnClickCallback_Map(string selectedMap)
    {
        selectedScene = selectedMap;
        for (int i = 0; i < listMaps.Count; ++i)
        {
            listMaps[i].Select(listMaps[i].MapName == selectedMap);
        }
    }
    #endregion

    #region Events
    public void OnClick_Close() => ViewManager.Instance.PopTop();

    public void OnClick_Start()
    {
        if (string.IsNullOrEmpty(selectedScene))
            return;

        // Set game configuration
        GameConfig.Instance.MapName = selectedScene;
        GameConfig.Instance.GameType = selectedGameType;
        switch (selectedGameType)
        {
            case GameManager.GameType.vsBot:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.AvatarId = selectedAvatar_playerA;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.Bot;
                //GameConfig.Instance.PlayerB.AvatarId = GameDefine.BOT_AVATAR_ID;
                // 6 is a dangerous number
                GameConfig.Instance.PlayerB.AvatarId = UnityEngine.Random.Range(0, 6);
                break;

            case GameManager.GameType.vsPlayer:
                GameConfig.Instance.PlayerA.Role = GameManager.GameTurn.P1;
                GameConfig.Instance.PlayerA.AvatarId = selectedAvatar_playerA;

                GameConfig.Instance.PlayerB.Role = GameManager.GameTurn.P2;
                GameConfig.Instance.PlayerB.AvatarId = selectedAvatar_playerB;
                break;
        }
        GameConfig.Instance.PrintDebugInfo();

        // Change view
        InGameView.CurrentScene = GameConfig.Instance.MapName;
        ViewManager.Instance.PopTop();
        ViewManager.Instance.ChangeMain(InGameView.Path);
        SceneManager.LoadScene(GameConfig.Instance.MapName, LoadSceneMode.Additive);
    }
    #endregion
}
