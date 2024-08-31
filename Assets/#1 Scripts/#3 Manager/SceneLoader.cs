using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

//레벨 이외의 호출 가능한 씬들
public enum Scenes
{
    MainMenu,
    main
}

public class SceneLoader : MonoBehaviour
{
    //현재 레벨 , n ~~~ = n레벨
    private int _currentLevel = 1;

    //레벨씬 전환
    public void ChangeLevel(int l, LoadSceneMode mode)
    {
        string level = "Level_" + l;
        //GameManager.Instance.isReset = false;
        StartCoroutine(LoadSceneAsync(level, mode));
        GameManager.Instance.SaveLevel(l);
        _currentLevel = l;
    }

    //일반적인 씬 전환
    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsync(scene.ToString(), mode));
    }

    //코루틴으로 로드해서 로딩 될때까지 기다리다가 씬 로드하기
    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene loaded: " + sceneName);
    }

    //기타 씬 로드 관련 함수들
    public void UnLoadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
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