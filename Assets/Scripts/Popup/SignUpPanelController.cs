using TMPro;
using UnityEngine;

public struct SignUpData
{
    public string username;
    public string password;
    public string nickname;
}



public class SignUpPanelController : PanelController
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    
    public void OnClickConfirmButton()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) ||
            string.IsNullOrEmpty(nickname))
        {
            Shake();
            return;
        }
        
        // confirm password 확인
        if (password.Equals(confirmPassword))
        {
            var signUpData = new SignUpData();
            signUpData.username = username;
            signUpData.password = password;
            signUpData.nickname = nickname;
            
            StartCoroutine(NetworkManager.Instance.SignUp(signUpData,
                () =>
                {
                    GameManager.Instance.OpenConfirmPanel("회원가입에 성공했습니다.", () =>
                    {
                        Hide();
                    });
                },
                (result) =>
                {
                    if (result == 0)
                    {
                        GameManager.Instance.OpenConfirmPanel("이미 존재하는 사용자입니다.", () =>
                        {
                            usernameInputField.text = "";
                            passwordInputField.text = "";
                            confirmPasswordInputField.text = "";
                            nicknameInputField.text = "";
                        });
                    }
                }));

            Hide(() =>
            {
                // TODO: 로그인 기능 구현
            });
        }
        else
        {
            
        }

        
    }

    public void OnClickCancelButton()
    {
        Hide();
    }
}
