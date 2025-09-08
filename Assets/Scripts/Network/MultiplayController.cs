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

public class MultiplayController
{
    private SocketIOUnity _socket;

    public MultiplayController()
    {
        var uri = new Uri(Constants.SocketServerURL);
        _socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        _socket.On("createRoom", CreateRoom );
        _socket.On("joinRoom", JoinRoom);
        _socket.On("startGame", StartGame);
        _socket.On("exitGame", ExitGame);
        _socket.On("endGame", EndGame);
        _socket.On("doOpponent", DoOpponent);
        
    }

    private void CreateRoom(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
        
    }

    private void JoinRoom(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
    }
    
    private void StartGame(SocketIOResponse obj)
    {
        var data = obj.GetValue<RoomData>();
    }
    
    private void ExitGame(SocketIOResponse obj)
    {
        
    }
    
    private void EndGame(SocketIOResponse obj)
    {
        
    }
    
    private void DoOpponent(SocketIOResponse obj)
    {
        var data = obj.GetValue<BlockData>();
    }
}
