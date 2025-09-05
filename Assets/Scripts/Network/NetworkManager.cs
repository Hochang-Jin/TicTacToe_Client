using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : Singleton<NetworkManager>
{
    // 로그인 
    public IEnumerator SignIn(SignInInfo signInInfo, Action success, Action<int> failure)
    {
        string jsonString = JsonUtility.ToJson(signInInfo);
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