using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIQuiz : UIBase
{
    private bool isInit = false;
    
    [Header("UI")]
    public TextMeshProUGUI questionText;
    public List<Button> choiceButtons;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI endText;

    [Header("설정")]
    public QuizType selectedType = QuizType.Science;

    private List<QuizDataSO> quizPool;
    private QuizDataSO currentQuiz;
    private Dictionary<QuizType, List<QuizDataSO>> remainingQuizzes = new();

    [SerializeField]private AudioUnit unit;
    
    private void OnEnable()
    {
        if (!isInit)
        {
            isInit = true;
            GameManager.Instance.isSolving = true;
            InitRemainingQuizPool();  
        }

        ShowNextQuiz();
    }

    
    private void InitRemainingQuizPool()
    {
        var source = GameManager.Instance.QuizDictionary;
        foreach (var kv in source)
        {
            remainingQuizzes[kv.Key] = new List<QuizDataSO>(kv.Value);
        }
    }

    public void ShowNextQuiz()
    {
        if (remainingQuizzes.Count == 0)
        {
            ShowEnd();
            return;
        }

        List<QuizType> availableTypes = new();
        foreach (var kv in remainingQuizzes)
        {
            if (kv.Value.Count > 0)
                availableTypes.Add(kv.Key);
        }

        if (availableTypes.Count == 0)
        {
            ShowEnd();
            return;
        }

        QuizType chosenType = availableTypes[Random.Range(0, availableTypes.Count)];
        List<QuizDataSO> pool = remainingQuizzes[chosenType];

        int index = Random.Range(0, pool.Count);
        currentQuiz = pool[index];
        pool.RemoveAt(index); 
        
        if (pool.Count == 0)
            remainingQuizzes.Remove(chosenType);

        DisplayQuiz(currentQuiz);
    }

    private void DisplayQuiz(QuizDataSO quiz)
    {
        questionText.text = quiz.question;
        typeText.text = $"[{quiz.type}]";

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            int idx = i;
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = quiz.choices[i];
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => CheckAnswer(idx));
        }
    }

    private void ShowEnd()
    {
        questionText.text = "모든 퀴즈가 끝났어요!";
        typeText.text = "";
        if (endText != null) endText.gameObject.SetActive(true);
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);
    }

    private void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == currentQuiz.correctAnswerIndex)
        {
            GameManager.Instance.PlayerController.audioUnit.PlaySFX(SFX.Correct);
            RunnerManager.Instance.CallOnQuizResult(true);
        }

        else
        {
            GameManager.Instance.PlayerController.audioUnit.PlaySFX(SFX.Wrong);
            RunnerManager.Instance.CallOnQuizResult(false);
        }
    
        UIManager.Instance.Hide<UIQuiz>();
        GameManager.Instance.isSolving = false;
    }


}