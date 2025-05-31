using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using Newtonsoft.Json;


public class LoginManager : MonoSingleton<LoginManager>
{
    
    public IEnumerator LoginRequest(string username, string password, Action<bool, string> callback)
    {
        string url = "https://edurun.shop/api/login";  

        var requestData = new LoginRequestData(username, password);
        string json = JsonConvert.SerializeObject(requestData);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(jsonBytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();


        if (req.result == UnityWebRequest.Result.Success)
        {
            try
            {
                string raw = req.downloadHandler.text;

                SaveLoginInfo(username, password); // 저장

                callback?.Invoke(true, raw);
            }
            catch (Exception e)
            {
                callback?.Invoke(false, "파싱 실패");
            }
        }
        else
        {
            Debug.LogError("❌ 서버 요청 실패: " + req.error);
            callback?.Invoke(false, "서버 요청 실패");
        }
    }


    
     
    public IEnumerator SignUpRequest(string username, string password, Action<bool, string> callback)
    {
        string json = JsonUtility.ToJson(new SignUpRequestData(username, password));
        string url = "https://edurun.shop/api/signup";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success &&
                (request.responseCode == 200 || request.responseCode == 201))
            {
                SaveLoginInfo(username,password);
                SignUpResponse response = JsonUtility.FromJson<SignUpResponse>(request.downloadHandler.text);
                callback?.Invoke(true, response.message);
            }
            else
            {
                callback?.Invoke(false, "회원가입 실패: " + request.downloadHandler.text);
            }
        }
    }
    
    //TODO 실사용시 암호화 필요 
    public void SaveLoginInfo(string id, string password)
    {
        ES3.Save("UserID", id);
        ES3.Save("UserPW", password);  
    }

    public (string, string) LoadLoginInfo()
    {
        string id = ES3.KeyExists("UserID") ? ES3.Load<string>("UserID") : "";
        string pw = ES3.KeyExists("UserPW") ? ES3.Load<string>("UserPW") : "";
        return (id, pw);
    }

    public void ClearLoginInfo()
    {
        ES3.DeleteKey("UserID");
        ES3.DeleteKey("UserPW");
    }

    
    
}


