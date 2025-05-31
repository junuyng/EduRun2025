using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class UserDataManager : MonoSingleton<UserDataManager>
{
    public IEnumerator GetUserItems(int userId, Action<List<UserItem>> callback)
    {
        string url = $"https://edurun.shop/api/user/items?userId={userId}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                try
                {
                    var items = JsonConvert.DeserializeObject<List<UserItem>>(json);
                    callback?.Invoke(items);
                }
                catch (Exception e)
                {
                    callback?.Invoke(null);
                }
            }
            else
            {
                callback?.Invoke(null);
            }
        }
    }
    
    public IEnumerator GetUserInfo(string username, Action<FullUserInfo> callback)
    {
        string url = $"https://edurun.shop/api/user/getUserInfo?username={username}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;

                try
                {
                    var userInfo = JsonConvert.DeserializeObject<FullUserInfo>(json);
                    callback?.Invoke(userInfo);
                }
                catch (Exception e)
                {
                    callback?.Invoke(null);
                }
            }
            else
            {
                callback?.Invoke(null);
            }
        }
    }
    
    
    public IEnumerator GetRankingList(Action<List<RankingEntry>> callback)
    {
        string url = "https://edurun.shop/api/user/ranking";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            
            
            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;

                try
                {
                    List<RankingEntry> entries = JsonConvert.DeserializeObject<List<RankingEntry>>(json);
                    callback?.Invoke(entries);
                }
                catch (Exception e)
                {
                    callback?.Invoke(null);
                }

            }
            else
            {
                callback?.Invoke(null);
            }
        }
    }

}