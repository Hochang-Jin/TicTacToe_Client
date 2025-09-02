using UnityEngine;

public class MainPanelController : MonoBehaviour
{
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.SINGLE);
    }

    public void OnClickDualPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.DUAL);
    }
    
    public void OnClickMultiPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.MULTI);
    }

    public void OnClickSettingButton()
    {
        
    }
}
