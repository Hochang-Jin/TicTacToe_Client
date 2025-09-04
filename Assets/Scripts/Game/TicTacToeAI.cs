using UnityEngine;

public static class TicTacToeAI
{
    /// <summary>
    /// 현재 상태를 전달하면 다음 최적의 수를 반환하는 메서드
    /// </summary>
    public static (int row, int col)? GetBestMove(Constants.PlayerType[,] board)
    {
        float bestScore = -1000;
        (int row, int col) movePosition = (-1, -1);

        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.NONE)
                {
                    board[row, col] = Constants.PlayerType.PLAYERB;
                    var score = TicTacToeAI.DoMiniMax(board, 0, false);
                    board[row, col] = Constants.PlayerType.NONE;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        movePosition = (row, col);
                    }
                }
            }
        }
        
        if( movePosition != (-1,-1))
            return movePosition;
        
        return null;
    }

    private static float DoMiniMax(Constants.PlayerType[,] board, int depth, bool isMaximizing)
    {
        // 게임 종료 상태 체크
        if (CheckGameWin(Constants.PlayerType.PLAYERA, board))
            return -10 + depth;
        if (CheckGameWin(Constants.PlayerType.PLAYERB, board))
            return 10 - depth;
        if (CheckGameDraw(board))
            return 0;

        if (isMaximizing)
        {
            var bestScore = float.MinValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.NONE)
                    {
                        board[row, col] = Constants.PlayerType.PLAYERB;
                        var score = DoMiniMax(board, depth + 1, false);
                        board[row, col] = Constants.PlayerType.NONE;
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            var bestScore = float.MaxValue;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.NONE)
                    {
                        board[row, col] = Constants.PlayerType.PLAYERA;
                        var score = DoMiniMax(board, depth + 1, true);
                        board[row, col] = Constants.PlayerType.NONE;
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }
    
    // 게임 승리 확인
    public static bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board)
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
    public static bool CheckGameDraw(Constants.PlayerType[,] board)
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
