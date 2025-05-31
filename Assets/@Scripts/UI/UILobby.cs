using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UILobby : UIBase
{
    private AudioUnit unit;

    //ButtonSetUp Elements
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image expBar;
    [SerializeField] private Image characterIcon;

    //Button Elements
    [SerializeField] private Button shopBtn;
    [SerializeField] private Button analysisBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button rankBtn;
    [SerializeField] private Button settingBtn;

  
    
    
    private void Start()
    {
        unit = GetComponent<AudioUnit>();
        
        
        ButtonSetUp();
        Init();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnBuy -= Init;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnBuy += Init;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBuy -= Init; 
    }

    private void Init()
    {
        string userName = ES3.Load<string>("UserID");

        // 전체 유저 정보 + 아이템 정보까지 새로고침
        StartCoroutine(UserDataManager.Instance.GetUserInfo(userName, SetUserInfo));
    }
    
    private void SetUserInfo(FullUserInfo info)
    {
        
        if (info == null)
        {
            return;
        }


        if (GameManager.Instance.userID == -1)
        {
            GameManager.Instance.userID = info.id;
        }

        var data = GameManager.Instance.playerData;
        
        data.userId = info.id;
        data.username = info.username;
        data.level = info.level;
        data.exp = info.exp;
        data.money = info.money;
        data.totalExp = info.totalexp;      
        data.userRank = info.userRank;

        
        levelText.text = info.level.ToString();
        goldText.text = info.money.ToString();
        expBar.fillAmount = info.exp / ((info.level + 1) * 100f);

        StartCoroutine(UserDataManager.Instance.GetUserItems(info.id, items =>
        {
            if (items != null)
            {
                GameManager.Instance.userItems = items;
                GameManager.Instance.CallOnUpdateCharacterInfo();
            }
            else
            {
            }
        }));
    }


    private void ButtonSetUp()
    {
        BindButton(shopBtn, () =>
        {
             UIManager.Instance.Show<UIShop>();
            unit.PlaySFX(SFX.ButtonLow);
        });
        BindButton(rankBtn, () =>
        {
            UIManager.Instance.Show<UIRanking>();
            unit.PlaySFX(SFX.ButtonLow);
        });
        BindButton(analysisBtn, () =>
        {
            UIManager.Instance.Show<UIAnalysis>();
            unit.PlaySFX(SFX.ButtonLow);
        });
        BindButton(settingBtn, () =>
        {
            UIManager.Instance.Show<UISettings>();
            unit.PlaySFX(SFX.ButtonLow);
        });
        
        BindButton(playBtn, PlayGame);
    }

    private void BindButton(Button button, UnityAction action)
    {
        if (button != null)
            button.onClick.AddListener(action);
    }


    private void PlayGame()
    {
        unit.PlaySFX(SFX.ButtonLow);
        UIManager.Instance.Hide<UILobby>();
        UIManager.Instance.Show<UILoading>().StartFakeOnlyLoading(2f);
        SceneManager.LoadScene("Game");


    }
}