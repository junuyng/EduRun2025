using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Lobby,
    Game
}

public class SceneLoader : MonoSingleton<SceneLoader>
{
    public void LoadScene(SceneName sceneName)
    {
        Logger.Log($"{sceneName} loading");

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName.ToString());
    }

    public void ReloadScene()
    {
        var name = SceneManager.GetActiveScene().name;
        Logger.Log($"{name} loading");
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    public AsyncOperation LoadSceneAsync(SceneName sceneName)
    {
        Logger.Log($"{sceneName} async loading");

        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneName.ToString());
    }

    public IEnumerator LoadSceneAsync(SceneName sceneName, Action<float> uiAction = null, Action callBack = null)
    {
        
        AsyncOperation operation = LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 씬 자동 활성화 방지
        float fakeLoadingTime = 2.0f;
        float elapsedFakeTime = 0f;
      
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            elapsedFakeTime += Time.deltaTime;

            float fakeProgress = Mathf.Clamp01(elapsedFakeTime / fakeLoadingTime);
            
            uiAction?.Invoke(fakeProgress);


            if (progress >= 0.9f && elapsedFakeTime >= fakeLoadingTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        uiAction?.Invoke(1f); 
        callBack?.Invoke(); 
        yield return new WaitForSeconds(0.5f); 
    }

}
