using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
 
[Serializable]
public class PlayerData
{
    public int userId;
    public string username;
    public int level;
    public int exp;
    public int money;
    public int totalExp;
    public int userRank;

    public List<UserItem> inventory = new List<UserItem>();
}


public class GameManager : MonoSingleton<GameManager>
{
   public bool hasToken = true;
   public int userID = -1;

   public bool isLoading = false;
   public bool isSolving = false;
   public string selectedSkin = "기본";
   
   public List<UserItem> userItems = new List<UserItem>();
   public PlayerData playerData = new PlayerData();

   public PlayerController PlayerController;
   
   public Dictionary<QuizType, List<QuizDataSO>> QuizDictionary { get; private set; } = new();
   
   public event Action OnBuy;
   public event Action OnUpdateCharacterInfo;
   public event Action OnRestartGame;
   public event Action OnCreateRunnerManager;
   
   
   protected override void Init()
    {
        base.Init();
        InitQuizData();
        TryAutoLogin();
        isDestroyOnLoad = true;
    }

    public void CallOnBuy()
    {
        OnBuy?.Invoke();
    }

    public void CallOnUpdateCharacterInfo()
    {
        OnUpdateCharacterInfo?.Invoke();
    }
    public void CallOnOnRestartGame()
    {
        OnRestartGame?.Invoke();
    }

    public void CallOnCreateRunnerManager()
    {
        OnCreateRunnerManager?.Invoke();
    }


    private void InitQuizData()
    {
        QuizDataSO[] allQuizzes = Resources.LoadAll<QuizDataSO>("QuizData");
        

        if (allQuizzes == null || allQuizzes.Length == 0)
        {
            return;
        }

        foreach (var quiz in allQuizzes)
        {
            if (!QuizDictionary.ContainsKey(quiz.type))
                QuizDictionary[quiz.type] = new List<QuizDataSO>();
            
            QuizDictionary[quiz.type].Add(quiz);
        }
        
    }


    
    private void TryAutoLogin()
    {
        if (ES3.KeyExists("UserID") && ES3.KeyExists("UserPW"))
        {
            string id = ES3.Load<string>("UserID");
            string pw = ES3.Load<string>("UserPW");


            StartCoroutine(LoginManager.Instance.LoginRequest(id, pw, (success, message) =>
            {
                if (success)
                {
                    UIManager.Instance.Show<UILobby>();
                    UIManager.Instance.Show<UILoading>().StartFakeOnlyLoading(2f);
                }
                else
                {
                }
            }));
        }
        else
        {
            UIManager.Instance.Show<UILogin>();
        }
    }
    
    public IEnumerator BuyItem(int userId, string itemName, Action<ItemBuyResponse> callback)
    {
        string url = "https://edurun.shop/api/itemBuy";

        var requestData = new
        {
            userId = userId,
            itemName = itemName
        };

        string json = JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");


        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {

            try
            {
                var response = JsonConvert.DeserializeObject<ItemBuyResponse>(req.downloadHandler.text);
                callback?.Invoke(response);
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

    
    public IEnumerator SendGameResultCoroutine(int winCount, Action<GameResultResponse> onComplete)
    {
        
        string url = "https://edurun.shop/api/problem/answer";

        var requestData = new
        {
            userId = userID,
            winCount = winCount
        };

        
        Debug.Log(userID.ToString());
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
                var response = JsonConvert.DeserializeObject<GameResultResponse>(req.downloadHandler.text);
                onComplete?.Invoke(response); 
            }
            catch (Exception e)
            {
                onComplete?.Invoke(null);
            }
        }
        else
        {
            onComplete?.Invoke(null);
        }
    }


 }