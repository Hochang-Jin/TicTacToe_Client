using System;
using Newtonsoft.Json;
using SocketIOClient;

public class RoomData
{
    [JsonProperty("roomId")] public string roomId { get; set; }
}

public class BlockData
{
    [JsonProperty("blockIndex")] public int blockIndex { get; set; }
}

public class MultiplayController : IDisposable
{
    private SocketIOUnity _socket;

    // Room 상태 변화에 따른 동작을 할당하는 변수
    private Action<Constants.MultiplayControllerState, string> _onMultiplayStateChanged; 
    // 게임 진행 상황에서 Marker의 위치를 업데이트 하는 변수
    public Action<int> onBlockChanged;
    
    public MultiplayController(Action<Constants.MultiplayControllerState, string> onMultiplayStateChanged)
    {
        _onMultiplayStateChanged = onMultiplayStateChanged;
        
        // socket.io 클라이언트 초기화
        var uri = new Uri(Constants.SocketServerURL);
        _socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        _socket.OnUnityThread("createRoom", CreateRoom );
        _socket.OnUnityThread("joinRoom", JoinRoom);
        _socket.OnUnityThread("startGame", StartGame);
        _socket.OnUnityThread("exitRoom", ExitRoom);
        _socket.OnUnityThread("endGame", EndGame);
        _socket.OnUnityThread("doOpponent", DoOpponent);
        
        _socket.Connect(); // 서버에 접속
    }

    #region Server -> Client
    private void CreateRoom(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayControllerState.CREATEROOM, data.roomId);
    }

    private void JoinRoom(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayControllerState.JOINROOM, data.roomId);
    }
    
    private void StartGame(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayControllerState.STARTGAME, data.roomId);
    }
    
    private void ExitRoom(SocketIOResponse obj)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayControllerState.EXITROOM, null);
    }
    
    private void EndGame(SocketIOResponse obj)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayControllerState.ENDGAME, null);
    }
    
    private void DoOpponent(SocketIOResponse obj)
    {
        var data = obj.GetValue<BlockData>();
        onBlockChanged?.Invoke(data.blockIndex);
    }
    #endregion

    #region Client -> Server

    // Room을 나올 떄 호출하는 메소드
    public void LeaveRoom(string roomId)
    {
        _socket.Emit("leaveRoom", new {roomId});
    }

    // 플레이어가 Marker를 두면 호출하는 메소드
    public void DoPlayer(string roomId, int position)
    {
        _socket.Emit("doPlayer", new {roomId, position});
    }

    #endregion

    public void Dispose()
    {
        if (_socket != null)
        {
            _socket.Disconnect();
            _socket.Dispose();
            _socket = null;
        }
    }
}
