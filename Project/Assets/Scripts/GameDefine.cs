using System.Collections.Generic;

public static class GameDefine
{
    public static readonly string DEFAULT_SCENE = "Map01_bot_Nghia";
    public static readonly int BOT_AVATAR_ID = 3;

    private static Dictionary<GameManager.GameType, Dictionary<GameManager.GameTurn, string>> dictDisplayName = 
        new Dictionary<GameManager.GameType, Dictionary<GameManager.GameTurn, string>>()
        {
            { GameManager.GameType.vsBot, new Dictionary<GameManager.GameTurn, string>{
                { GameManager.GameTurn.Bot, "Bot" },
                { GameManager.GameTurn.P1,  "Player" }
            }},
            { GameManager.GameType.vsPlayer, new Dictionary<GameManager.GameTurn, string>{
                {GameManager.GameTurn.P1, "Player 1"},
                {GameManager.GameTurn.P2, "Player 2"},
            }},
        };

    public static string GetPlayerDisplayName(GameManager.GameType gameType, GameManager.GameTurn gameTurn)
    {
        string displayName = "";
        if (dictDisplayName.TryGetValue(gameType, out var dict))
        {
            dict.TryGetValue(gameTurn, out displayName);
        }
        return displayName;
    }
}
