using System;
using UnityEngine;

public class GameLogic : IDisposable
{
    public BlockController blockController; // Block을 처리할 객체

    private Constants.PlayerType[,] _board; // 보드의 상태 정보
    private BasePlayerState _currentPlayerState;

    public BasePlayerState firstPlayerState;       // Player A
    public BasePlayerState secondPlayerState;      // Player B
    
    public enum GameResult{ NONE, WIN, LOSE, DRAW }
    
    private MultiplayController _multiplayController;
    private string _roomId;
    
    // 생성자
    public GameLogic(BlockController blockController, Constants.GameType gameType)
    {
        this.blockController = blockController;
        
        // 보드의 상태 정보 초기화
        _board = new Constants.PlayerType[Constants.BlockColumnCount, Constants.BlockColumnCount];
        
        // Game Type 초기화
        switch (gameType)
        {
            case Constants.GameType.SINGLE:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new AiState();
                // 게임 시작
                SetState(firstPlayerState);
                break;
            case Constants.GameType.DUAL:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new PlayerState(false);
                // 게임 시작
                SetState(firstPlayerState);
                break;
            case Constants.GameType.MULTI:
                _multiplayController = new MultiplayController((state, roomId) =>
                {
                    _roomId = roomId;
                    switch (state)
                    {
                        case Constants.MultiplayControllerState.CREATEROOM:
                            Debug.Log("### CREATE ROOM ###");
                            // TODO: 대기 화면
                            break;
                        case Constants.MultiplayControllerState.JOINROOM:
                            Debug.Log("### JOIN ROOM ###");
                            firstPlayerState = new MultiplayerState(true, _multiplayController);
                            secondPlayerState = new PlayerState(false, _multiplayController, _roomId);
                            SetState(firstPlayerState);
                            break;
                        case Constants.MultiplayControllerState.STARTGAME:
                            Debug.Log("### START GAME ###");
                            firstPlayerState = new PlayerState(true, _multiplayController, _roomId);
                            secondPlayerState = new MultiplayerState(false, _multiplayController);
                            SetState(firstPlayerState);
                            break;
                        case Constants.MultiplayControllerState.EXITROOM:
                            Debug.Log("### EXIT ROOM ###");
                            // TODO: 팝업 띄우고 메인화면으로 이동
                            break;
                        case Constants.MultiplayControllerState.ENDGAME:
                            Debug.Log("### END GAME ###");
                            // TODO: 팝업 띄우고 메인화면으로 이동
                            break;
                    }
                });
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
        
        // 유저에게 Game Over 패널 표시
        GameManager.Instance.OpenConfirmPanel("게임 오버", () =>
        {
            GameManager.Instance.ChangeToMainScene();
        });
    }
    
    // 게임의 결과 확인
    public GameResult CheckGameResult()
    {
        if(TicTacToeAI.CheckGameWin(Constants.PlayerType.PLAYERA, _board))
            return GameResult.WIN;
        if(TicTacToeAI.CheckGameWin(Constants.PlayerType.PLAYERB, _board))
            return GameResult.LOSE;
        if(TicTacToeAI.CheckGameDraw(_board))
            return GameResult.DRAW;
        return GameResult.NONE;
    }
    
    // _board Getter
    public Constants.PlayerType[,] GetBoard()
    {
        return _board;
    }

    public void Dispose()
    {
        _multiplayController?.LeaveRoom(_roomId);
        _multiplayController?.Dispose();
    }
}
