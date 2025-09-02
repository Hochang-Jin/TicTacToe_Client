using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private TMP_Text messageText;

    /// <summary>
    /// Confirm Panel을 표시하는 메소드
    /// </summary>
    /// <param name="message"></param>
    public void Show(string message)
    {
        messageText.text = message;
        base.Show();
    }
    
    /// <summary>
    /// 확인 버튼 눌렀을 때 호출되는 메소드
    /// </summary>
    public void OnClickConfirmButton()
    {
        Hide();
        SceneManager.LoadScene("Main");
    }
    
    /// <summary>
    /// x 버튼 눌렀을 때 호출되는 메소드 
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}
