using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartData
{
    private static GameStartData _instance;

    private GameStartData() { }
    public static GameStartData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameStartData();
            }
            return _instance;
        }
    }

    public GameManager.GameType gameType;
    public ConfigAvatarData P1;
    public ConfigAvatarData P2;
    public string MapName = "";
}
