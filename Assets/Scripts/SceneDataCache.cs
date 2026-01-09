using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneDataCache
{
    public static int selectedStage = -1;
    public static bool canCloseContinuePopup = true;
    public static bool allowNextSceneActivation = false;
    public static AsyncOperation PreloadedScene;

    public static void LoadScene(bool directload, string scene, int areaNum)
    {
        if (directload)
        {
            Debug.Log("LoadingScene: " + scene);
            SceneManager.LoadScene(scene);
        }
        else
        {
            selectedStage = areaNum;
            if (scene == "Stage")
            {
                Debug.Log("LoadingScene: " + scene + "_" + areaNum);
                SceneManager.LoadScene(scene + "_" + areaNum);
            }
            else
            {
                Debug.LogError("NotFoundScene: " + scene + "_" + areaNum);
            }
        }
    }

    public static void PreLoadSceneAsync(bool directload, string scene, int areaNum)
    {
        if (directload)
        {
            Debug.Log("LoadingScene: " + scene);
            SceneManager.LoadScene(scene);
        }
        else
        {
            selectedStage = areaNum;
            if (scene == "Stage")
            {
                Debug.Log("PreLoadingScene: " + scene + "_" + areaNum + " (Async)");
                PreloadedScene = SceneManager.LoadSceneAsync(scene + "_" + areaNum);
            }
            else
            {
                Debug.LogError("NotFoundScene: " + scene + "_" + areaNum);
            }
        }
        PreloadedScene.allowSceneActivation = false;
    }

    public static void LoadSceneAsync()
    {
        allowNextSceneActivation = false;
        PreloadedScene.allowSceneActivation = true;
    }

    public static IEnumerator WaitForSomeTimes(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

}
