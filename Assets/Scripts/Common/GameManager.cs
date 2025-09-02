using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject confirmPanel;
    
    // Main Scene에서 선택한 게임 타입
    private Constants.GameType _gameType;
    
    // Panel을 띄우기 위한 Canvas 정보
    private Canvas _canvas;
    
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

    /// <summary>
    /// Confirm Panel을 띄우는 메소드
    /// </summary>
    /// <param name="message"></param>
    public void OpenConfirmPanel(string message)
    {
        if (_canvas != null)
        {
            var confirmPanelObject = Instantiate(confirmPanel, _canvas.transform);
            confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message);
        }
    }
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // TODO : 씬 전환 시 처리할 함수
        _canvas = FindFirstObjectByType<Canvas>();
    }
}
