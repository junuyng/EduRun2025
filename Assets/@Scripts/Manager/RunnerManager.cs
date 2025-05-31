using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoSingleton<RunnerManager>
{
    private bool hasGameStarted = false;
    private float quizTimer = 0f;
    private float quizInterval = 6f;
    public event Action OnStartGame;
    public event Action<bool> OnQuizResult;
    [SerializeField] private GameObject playerPrefabs;
    public PlayerController PlayerController;

    public int SolvedQuizNum { get; private set; }

    protected override void Init()
    {
        isDestroyOnLoad = true;
        PlayerController = Instantiate(playerPrefabs).GetComponent<PlayerController>();
        base.Init();
        Time.timeScale = 0f;
    }
    

    private void Update()
    {
        if (!hasGameStarted && IsTouchStarted() && !GameManager.Instance.isLoading)
        {
            GameManager.Instance.CallOnCreateRunnerManager();
            StartGame();
        }
        
        if (hasGameStarted)
        {
            quizTimer += Time.deltaTime;

            if (quizTimer >= quizInterval)
            {
                quizTimer = 0f;
                ShowQuiz();
            }
        }
    }

    private bool IsTouchStarted()
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }

    private void StartGame()
    {
        hasGameStarted = true;
        Time.timeScale = 1f;
        OnStartGame?.Invoke();
    }

    private void ShowQuiz()
    {
        Time.timeScale = 0f;
        UIManager.Instance.Show<UIQuiz>();
    }

    public void CallOnQuizResult(bool result)
    {
        Time.timeScale = 1f;
        if (result)
            SolvedQuizNum++;
        
        OnQuizResult?.Invoke(result);
    }
}

