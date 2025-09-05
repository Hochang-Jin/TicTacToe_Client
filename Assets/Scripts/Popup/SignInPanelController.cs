using DG.Tweening;
using TMPro;
using UnityEngine;

public struct SignInInfo
{
    public string username;
    public string password;
}

public struct SignInResult
{
    public int result;
}

public class SignInPanelController : PanelController
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    public void OnClickConfirmButton()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // TODO: 누락된 값을 입력하도록 요청
            Shake();
            return;
        }

        var signInInfo = new SignInInfo();
        signInInfo.username = username;
        signInInfo.password = password;

        StartCoroutine(NetworkManager.Instance.SignIn(signInInfo,
            () => { Hide(); },
            (result) =>
            {
                if (result == 0)
                {
                    GameManager.Instance.OpenConfirmPanel("유저네임이 유효하지 않습니다.", () => { });
                }
                else if (result == 1)
                {
                    GameManager.Instance.OpenConfirmPanel("패스워드가 유효하지 않습니다.", () => { });
                }
            }));

        Hide(() =>
        {
            // TODO: 로그인 기능 구현
        });
    }

}
