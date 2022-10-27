using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
#if UNITY_EDITOR
    public void Player1Win() => CheatEndGame(GameManager.GameEndType.Player1Win);

    public void Player2Win() => CheatEndGame(GameManager.GameEndType.Player2Win);

    public void PlayerWin() => CheatEndGame(GameManager.GameEndType.PlayerWin);

    public void BotWin() => CheatEndGame(GameManager.GameEndType.BotWin);

    private void CheatEndGame(GameManager.GameEndType gameEndType)
    {
        Time.timeScale = 0f;
        EndGameView.GameEndType = gameEndType;
        ViewManager.Instance.PushTop(EndGameView.Path);
    }
#else
    void Start()
    {
        gameObject.SetActive(false);
    }
#endif
}
