using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

public enum Scenes
{
    MainMenu,
    Setting,
    EscMenu,
}

public class SceneLoader : MonoBehaviour
{
    //현재 레벨 , n ~~~ = n레벨
    private int _currentLevel = 1;

    public void ChangeLevel(int l, LoadSceneMode mode)
    {
        string level = "Level_" + l;
        StartCoroutine(LoadSceneAsync(level, mode));
        _currentLevel = l;
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsync(scene.ToString(), mode));
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene loaded: " + sceneName);
    }
    
    

    public void UnLoadScene(Scenes scene)
    {
        SceneManager.UnloadSceneAsync(scene.ToString());
    }

    public bool CheckLoadScene(string scene)
    {
        return SceneManager.GetSceneByName(scene).isLoaded;
    }
    public bool CheckLoadScene(Scenes scene)
    {
        return SceneManager.GetSceneByName(scene.ToString()).isLoaded;
    }
    
}