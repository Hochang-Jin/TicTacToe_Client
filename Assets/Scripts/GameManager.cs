using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Constants.GameType _gameType;
    
    /// <summary>
    /// Main -> Game Scene으로 전환 시 호출 될 메소드
    /// </summary>
    /// <param name="gameType">0: Single, 1: Dual, 2: Multi</param>
    public void ChangeToGameScene(Constants.GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Game -> Main Scene으로 전환 시 호출 될 메소드
    /// </summary>
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // TODO : 씬 전환 시 처리할 함수
    }
}
