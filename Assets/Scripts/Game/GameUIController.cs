using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject playerATurnPanel;
    [SerializeField] private GameObject playerBTurnPanel;
    [SerializeField] private TMP_Text gameTypeText;
    
    public enum GameTurnPanelType { NONE, ATURN, BTURN }
    
    public void OnClickBackButton()
    {
        GameManager.Instance.OpenConfirmPanel("게임을 종료하시겠습니까?", () =>
        {
            GameManager.Instance.ChangeToMainScene();
        });
    }

    public void SetGameTurnPanel(GameTurnPanelType gameTurnPanelType)
    {
        switch (gameTurnPanelType)
        {
            case GameTurnPanelType.NONE:
                playerATurnPanel.SetActive(false);
                playerBTurnPanel.SetActive(false);
                break;
            case GameTurnPanelType.ATURN:
                playerATurnPanel.SetActive(true);
                playerBTurnPanel.SetActive(false);
                break;
            case GameTurnPanelType.BTURN:
                playerATurnPanel.SetActive(false);
                playerBTurnPanel.SetActive(true);
                break;
        }
    }

    public void SetGameType(Constants.GameType gameType)
    {
        switch (gameType)
        {
            case Constants.GameType.SINGLE:
                gameTypeText.text = "싱글 플레이";
                break;
            case Constants.GameType.DUAL:
                gameTypeText.text = "듀얼 플레이";
                break;
            case Constants.GameType.MULTI:
                gameTypeText.text = "멀티 플레이";
                break;
        }
    }
}
