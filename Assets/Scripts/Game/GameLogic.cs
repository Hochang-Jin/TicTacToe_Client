using System;
using UnityEngine;

public class GameLogic
{
    public BlockController blockController; // Block을 처리할 객체

    private Constants.PlayerType[,] _board; // 보드의 상태 정보
    private BasePlayerState _currentPlayerState;

    public BasePlayerState firstPlayerState;       // Player A
    public BasePlayerState secondPlayerState;      // Player B
    
    public enum GameResult{ NONE, WIN, LOSE, DRAW }
    
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="blockController"></param>
    /// <param name="gameType"></param>
    public GameLogic(BlockController blockController, Constants.GameType gameType)
    {
        this.blockController = blockController;
        
        // 보드의 상태 정보 초기화
        _board = new Constants.PlayerType[Constants.BlockColumnCount, Constants.BlockColumnCount];
        
        // Game Type 초기화
        switch (gameType)
        {
            case Constants.GameType.SINGLE:
                break;
            case Constants.GameType.DUAL:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new PlayerState(false);
                
                // 게임 시작
                SetState(firstPlayerState);
                break;
            case Constants.GameType.MULTI:
                break;
        }
    }

    // 턴이 바뀔 때, 기존 상태를 Exit 하고 새 상태를 currentPlayerState에 할당하고 이번 상태에 Enter
    public void SetState(BasePlayerState state)
    {
        _currentPlayerState?.OnExit(this);
        _currentPlayerState = state;
        _currentPlayerState?.OnEnter(this);
    }
    
    // _board 배열에 새로운 marker 값을 할당
    public bool SetNewBoardValue(Constants.PlayerType playerType, int row, int col)
    {
        if (_board[row, col] != Constants.PlayerType.NONE) return false;

        if (playerType == Constants.PlayerType.PLAYERA)
        {
            _board[row, col] = playerType;
            blockController.SetBlockMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if(playerType == Constants.PlayerType.PLAYERB)
        {
            _board[row, col] = playerType;
            blockController.SetBlockMarker(Block.MarkerType.X, row, col);
            return true;
        }

        return false;
    }
    
    // Game Over 처리
    public void EndGame(GameResult gameResult)
    {
        SetState(null);
        firstPlayerState = null;
        secondPlayerState = null;
        
        // TODO: 유저에게 Game Over 패널 표시
        Debug.Log("Game OVER");
    }
    
    // 게임의 결과 확인
    public GameResult CheckGameResult()
    {
        if(CheckGameWin(Constants.PlayerType.PLAYERA, _board))
            return GameResult.WIN;
        if(CheckGameWin(Constants.PlayerType.PLAYERB, _board))
            return GameResult.LOSE;
        if(CheckGameDraw(_board))
            return GameResult.DRAW;
        return GameResult.NONE;
    }
    
    // 게임 승리 확인
    private bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board)
    {
        // Col 체크 후 일자면 true
        for (var row = 0; row < Constants.BlockColumnCount; row++)
        {
            if (board[row, 0] == playerType && board[row, 1] == playerType && board[row, 2] == playerType) return true;
        } 
        // Row 체크 후 일자면 true
        for (var col = 0; col < Constants.BlockColumnCount; col++)
        {
            if(board[0,col] == playerType && board[1,col] == playerType && board[2,col] == playerType) return true;
        }
        // 대각 체크 후 일자면 true
        if(board[0,0] == playerType && board[1,1] == playerType && board[2,2] == playerType) return true;
        if(board[0,2] == playerType && board[1,1] == playerType && board[2,0] == playerType) return true;

        return false;
    }
    
    // 비겼는지 확인
    private bool CheckGameDraw(Constants.PlayerType[,] board)
    {
        for (var row = 0; row < Constants.BlockColumnCount; row++)
        {
            for (var col = 0; col < Constants.BlockColumnCount; col++)
            {
                if(board[row, col] == Constants.PlayerType.NONE) return false;
            }
        }
        return true;
    }
}
