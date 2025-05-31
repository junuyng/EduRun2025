using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIBase
{
    [SerializeField]private GameObject StartMsg;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBar_Gradi;
  

    private void OnEnable()
    {
        ResetUI();
    }

    private void Start()
    {
        StartMsg.SetActive(true);
        UpdateHpUI(RunnerManager.Instance.PlayerController.MaxHealth);

        GameManager.Instance.OnRestartGame += ResetUI;
        GameManager.Instance.OnCreateRunnerManager += LinkRunnerManager;
    }

    private void UpdateHpUI(float value)
    {
        float amount = value / RunnerManager.Instance.PlayerController.MaxHealth;
        hpText.text = (amount * 100).ToString() + "%";
        hpBar.fillAmount = amount;
        hpBar_Gradi.fillAmount = amount;
    }
    
    public void ResetUI()
    {
        StartMsg.SetActive(true);
        AudioMixerController.Instance.PlayBGM(BGM.Runner);
        UpdateHpUI(RunnerManager.Instance.PlayerController.MaxHealth);
      
    }

    private void LinkRunnerManager()
    {
        
        RunnerManager.Instance.OnStartGame += () =>
        {
            StartMsg.SetActive(false);
        };
        RunnerManager.Instance.PlayerController.OnHit += UpdateHpUI;

    }

}
