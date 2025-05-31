using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEndGame : UIBase
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private TextMeshProUGUI exp;
    [SerializeField] private TextMeshProUGUI solvedQuiz;
    [SerializeField] private TextMeshProUGUI gold;

    private void Start()
    {
        restartBtn.onClick.AddListener(Restart);
        exitBtn.onClick.AddListener(ExitGame);
        StartCoroutine(GameManager.Instance.SendGameResultCoroutine(RunnerManager.Instance.SolvedQuizNum, SetUI));
    }
    

    private void SetUI(GameResultResponse result)
    {
        if (result == null)
        {
            return;
        }
        
        GameManager.Instance.CallOnOnRestartGame();
        solvedQuiz.text = RunnerManager.Instance.SolvedQuizNum + "문제";
        exp.text = result.gained.exp.ToString();
        gold.text = result.gained.money.ToString();
    }

    private void Restart()
    {
        Destroy(RunnerManager.Instance.gameObject);
        SceneManager.LoadScene("Game");
        UIManager.Instance.Hide<UIEndGame>();
    }

    private void ExitGame()
    {
        
        Destroy(RunnerManager.Instance.gameObject);
        SceneManager.LoadScene("Lobby");
        UIManager.Instance.Hide<UIEndGame>();
        UIManager.Instance.Hide<UIInGame>();
        UIManager.Instance.Show<UILobby>();
        UIManager.Instance.Show<UILoading>().StartFakeOnlyLoading(2f);
    }
}