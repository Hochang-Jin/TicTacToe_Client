public class PlayerState : BasePlayerState
{
    private bool _isFirstPlayer;
    private Constants.PlayerType _playerType;

    private MultiplayController _multiplayController;
    private string _roomId;
    private bool _isMultiplay;
    
    public PlayerState(bool isFirstPlayer)
    {
        _isFirstPlayer = isFirstPlayer;
        _playerType = _isFirstPlayer ? Constants.PlayerType.PLAYERA : Constants.PlayerType.PLAYERB;
        _isMultiplay = false;
    }

    public PlayerState(bool isFirstPlayer, MultiplayController multiplayController, string roomId) : this(isFirstPlayer)
    {
        _multiplayController = multiplayController;
        _roomId = roomId;
        _isMultiplay = true;
    }

#region 필수 메소드
    
    public override void OnEnter(GameLogic gameLogic)
    {
        // 1. First Player인지 확인 후 게임 UI에 현재 턴 표시
        if(_isFirstPlayer)
            GameManager.Instance.SetGameTurnPanel(GameUIController.GameTurnPanelType.ATURN);
        else
            GameManager.Instance.SetGameTurnPanel(GameUIController.GameTurnPanelType.BTURN);
        
        // 2. Block Controller에게 해야 할 일을 전달
        gameLogic.blockController.OnBlockClickedDelegate = (row, col) =>
        {
            // Block이 터치 될 때 까지 기다렸다가 터치 되면 처리할 일
            HandleMove(gameLogic, row, col);
        };
    }

    public override void OnExit(GameLogic gameLogic)
    {
        gameLogic.blockController.OnBlockClickedDelegate = null;
    }

    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        ProcessMove(gameLogic, _playerType, row, col);

        if (_isMultiplay) // 서버에 Marker 정보 전달
        {
            _multiplayController.DoPlayer(_roomId, row * Constants.BlockColumnCount + col);
        }
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        if (_isFirstPlayer)
        {
            gameLogic.SetState(gameLogic.secondPlayerState);
        }

        else
        {
            gameLogic.SetState(gameLogic.firstPlayerState);
        }
    }
    
    #endregion
}
