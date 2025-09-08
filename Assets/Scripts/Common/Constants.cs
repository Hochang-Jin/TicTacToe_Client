using UnityEngine;

public static class Constants
{
    public enum GameType { SINGLE, DUAL, MULTI }
    public enum PlayerType { NONE, PLAYERA, PLAYERB }
    
    public enum MultiplayControllerState{CREATEROOM, JOINROOM, STARTGAME, EXITROOM, ENDGAME}

    public const int BlockColumnCount = 3;
    
    public const string ServerURL = "http://localhost:3000";
    public const string SocketServerURL = "ws://localhost:3000";
}
