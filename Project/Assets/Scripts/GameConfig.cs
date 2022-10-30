using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public GameManager.GameTurn Role;
    public string Avatar;
}

public class GameConfig
{
    public static GameConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameConfig();
            }
            return _instance;
        }
    }

    private static GameConfig _instance;
    private GameConfig()
    {
        PlayerA = new PlayerInfo();
        PlayerB = new PlayerInfo();
    }

    public GameManager.GameType GameType;

    /// <summary>
    /// Player A will be user player if game type is vs bot, and will be player 1 if game type is vs other human player
    /// </summary>
    public PlayerInfo PlayerA;

    /// <summary>
    /// Player B will be bot if game type is vs bot, and will be player 2 if game type is vs other human player
    /// </summary>
    public PlayerInfo PlayerB;
}
