using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoading : UIBase
{
   
    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBar_gradi;
    [SerializeField] private TextMeshProUGUI percentText;
    
    
    public void StartFakeOnlyLoading(float duration = 1f, Action onComplete = null)
    {
        AudioMixerController.Instance.StopBGM();
        GameManager.Instance.isLoading = true;
        StartCoroutine(FakeOnlyLoadingRoutine(duration, onComplete));
    }

    private IEnumerator FakeOnlyLoadingRoutine(float duration, Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // ⬅️ 시간 스케일 무시
            float progress = Mathf.Clamp01(elapsed / duration);
            SetProgress(progress);
            yield return null;
        }

        SetProgress(1f);
        yield return new WaitForSecondsRealtime(0.2f); // 완료감 강조를 위한 잠깐 딜레이

        onComplete?.Invoke();

      
        if (SceneManager.GetActiveScene().name == "Game" )
        {
            UIManager.Instance.Show<UIInGame>();
        }
        
        UIManager.Instance.Hide<UILoading>();
        GameManager.Instance.isLoading = false;

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            AudioMixerController.Instance.PlayBGM(BGM.Lobby);
        }

    }

    public void StartFakeLoading(SceneName targetScene)
    {
        StartCoroutine(FakeProgressRoutine(targetScene));
    }

    private IEnumerator FakeProgressRoutine(SceneName sceneName)
    {
        float fakeProgress = 0f;
        float realProgress = 0f;
        bool sceneLoaded = false;

        // 씬 비동기 로드 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
        operation.allowSceneActivation = false;

        while (!sceneLoaded)
        {
            // 실제 진행률 (Unity가 제공)
            realProgress = operation.progress / 0.9f; // 유니티는 0.9까지만 가고 멈춤

            // 페이크 진행률 (천천히 증가하다가 실제 진행률 따라잡음)
            fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 0.5f);

            SetProgress(fakeProgress);

            // 둘이 거의 같아지고 실제 로딩도 완료되면 씬 전환
            if (fakeProgress >= 0.99f && realProgress >= 0.89f) // 실수 오차 감안
            {
                operation.allowSceneActivation = true;
                sceneLoaded = true;
            }
            yield return null;
        }

        // 실제 씬 적용
        operation.allowSceneActivation = true;
        
    }

    public void SetProgress(float progress)
    {
        progressBar.fillAmount = progress;
        progressBar_gradi.fillAmount = progress;
        percentText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }


    
}
