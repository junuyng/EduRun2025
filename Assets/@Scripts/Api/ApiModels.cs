// ApiModels.cs

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class LoginRequestData
{
    public string username;
    public string password;

    public LoginRequestData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
[Serializable]
public class ItemBuyResponse
{
    public string message;
    public int userId;
    public int itemId;
}


[Serializable]
public class UserItem
{
    [JsonProperty("itemId")] public int itemId;
    [JsonProperty("itemName")] public string itemName;
    [JsonProperty("description")] public string description;
    [JsonProperty("price")] public int price;
    [JsonProperty("quantity")] public int quantity;
}

[Serializable]
public class FullUserInfo
{
    [JsonProperty("userId")] public int id;
    [JsonProperty("userName")] public string username;
    [JsonProperty("level")] public int level;
    [JsonProperty("exp")] public int exp;
    [JsonProperty("money")] public int money;
    [JsonProperty("userRank")] public int userRank;
    [JsonProperty("totalexp")] public int totalexp;
}


[Serializable]
public class SignUpRequestData
{
    public string username;
    public string password;
    
    public SignUpRequestData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class SignUpResponse
{
    public int id;
    public string username;
    public string message;
}

[Serializable]
public class UserInfo
{
    public int id;
    public string username;
}

[Serializable]
public class LoginSuccessResponse
{
    public string message;
    public UserInfo user;
}

[Serializable]
public class LoginErrorResponse
{
    public string errorCode;
    public string message;
}

[Serializable]
public class RankingEntry
{
    public int userId;
    public string userName;
    public int userRank;
    public int totalexp;
    public int level;
    public int exp;
}

[Serializable]
public class RankingResponse
{
    public List<RankingEntry> rankings;
    public string message;
}

[Serializable]
public class GameResultResponse
{
    public string message;
    public int userId;
    public GameStat before;
    public GameStat gained;
    public GameStat after;
}

[Serializable]
public class GameStat
{
    public int exp;
    public int money;
}