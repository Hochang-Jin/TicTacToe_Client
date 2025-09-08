using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : Singleton<NetworkManager>
{
    // 회원가입
    public IEnumerator SignUp(SignUpData signUpData, Action success, Action<int> failure)
    {
        string jsonString = JsonUtility.ToJson(signUpData);
        byte[] byteRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(Constants.ServerURL + "/users/signup",
                   UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(byteRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // TODO: 서버 연결 오류 알림
            }
            else
            {
                var resultString = www.downloadHandler.text;
                var result = JsonUtility.FromJson<SignInResult>(resultString);

                // Sign Up Success
                if (result.result == 2)
                {
                    success?.Invoke();
                }
                else
                {
                    failure?.Invoke(result.result);
                }
            }
        }
    }
    
    
    // 로그인 
    public IEnumerator SignIn(SignInData signInData, Action success, Action<int> failure)
    {
        string jsonString = JsonUtility.ToJson(signInData);
        byte[] byteRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        // 객체가 사용 될 범위를 지정하고, 사용 후 GC가 정리
        using (UnityWebRequest www = new UnityWebRequest(Constants.ServerURL + "/users/signin",
            UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(byteRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // TODO: 서버 연결 오류 알림
            }
            else
            {
               var resultString = www.downloadHandler.text;
               var result = JsonUtility.FromJson<SignInResult>(resultString);

               // 0: INVALID_USERNAME, 1: INVALID_PASSWORD, 2: SUCCESS
               if (result.result == 2)
               {
                   // 로그인 성공
                   var cookie = www.GetResponseHeader("set-cookie");
                   if (!string.IsNullOrEmpty(cookie))
                   {
                       // sid 파싱
                       int lastIndex = cookie.LastIndexOf(';');
                       string sid = cookie.Substring(0, lastIndex);
                       // 저장
                       PlayerPrefs.SetString("sid", sid);
                   }
                   success?.Invoke();
               }
               else 
               {
                   failure.Invoke(result.result);
               }
            }
        }

    }
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        
    }
}