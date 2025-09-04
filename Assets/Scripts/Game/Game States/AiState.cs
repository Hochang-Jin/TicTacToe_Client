using UnityEngine;

public class AiState : BasePlayerState
{
    
    #region 필수 함수 구현
    public override void OnEnter(GameLogic gameLogic)
    {
        // 턴 표시
        GameManager.Instance.SetGameTurnPanel(GameUIController.GameTurnPanelType.BTURN);

        // AI 연산
        var board = gameLogic.GetBoard();
        var result = TicTacToeAI.GetBestMove(board);
        if (result.HasValue)
        {
            HandleMove(gameLogic,result.Value.row,result.Value.col);
        }
        else
        {
            gameLogic.EndGame(GameLogic.GameResult.DRAW);
        }
    }

    public override void OnExit(GameLogic gameLogic)
    {
        
    }

    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        ProcessMove(gameLogic, Constants.PlayerType.PLAYERB, row, col);
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        gameLogic.SetState(gameLogic.firstPlayerState);
    }    
    #endregion
}
