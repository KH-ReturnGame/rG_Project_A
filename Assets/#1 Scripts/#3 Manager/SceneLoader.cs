using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//레벨 이외의 호출 가능한 씬들
public enum Scenes
{
    MainMenu,
    main,
    EndingScene,
    SceneLoad,
}

public class SceneLoader : MonoBehaviour
{
    //현재 레벨 , n ~~~ = n레벨
    private int _currentLevel = 1;

    private string _nextScene;
    private LoadSceneMode _nextMode;

    //레벨씬 전환
    public void ChangeLevel(int l, LoadSceneMode mode)
    {
        _nextScene = "Level_" + l;
        _nextMode = mode;
        StartCoroutine(LoadScene());
        
        GameManager.Instance.SaveLevel(l);
        _currentLevel = l;
    }

    //일반적인 씬 전환
    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        _nextScene = scene.ToString();
        _nextMode = mode;
        StartCoroutine(LoadScene());
    }

    //코루틴으로 로드해서 로딩 될때까지 기다리다가 씬 로드하기
    IEnumerator LoadScene()
    {
        GameManager.Instance.isLoding = true;
        yield return SceneManager.LoadSceneAsync("SceneLoad", LoadSceneMode.Single);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nextScene,_nextMode);
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = false;

            float timer = 0f;
            while (true)
            {
                yield return null;
                //Debug.Log(asyncLoad.progress);
                if (asyncLoad.progress < 0.9f)
                {
                    GameObject.FindGameObjectWithTag("loding").GetComponent<Image>().fillAmount = asyncLoad.progress;
                }
                else
                {
                    timer += Time.deltaTime;
                    GameObject.FindGameObjectWithTag("loding").GetComponent<Image>().fillAmount =
                        Mathf.Lerp(0.9f, 1f, timer);
                    if (GameObject.FindGameObjectWithTag("loding").GetComponent<Image>().fillAmount >= 1f)
                    {
                        asyncLoad.allowSceneActivation = true;
                        Debug.Log("Scene loaded: " + _nextScene);
                        GameManager.Instance.isLoding = false;
                        yield break;
                    }
                }
            }
        }
    }
    
    public void LoadMainAndLevel(int level)
    {
        StartCoroutine(LoadMainAndLevelCoroutine(level));
    }
    
    IEnumerator LoadMainAndLevelCoroutine(int level)
    {
        GameManager.Instance.isLoding = true;
        Debug.Log("Starting LoadMainAndLevelCoroutine for level: " + level);

        yield return SceneManager.LoadSceneAsync("SceneLoad", LoadSceneMode.Single);
        Debug.Log("SceneLoad loaded");

        AsyncOperation mainLoadOperation = SceneManager.LoadSceneAsync("main", LoadSceneMode.Additive);
        mainLoadOperation.allowSceneActivation = false;
        Debug.Log("Started loading main scene");

        AsyncOperation levelLoadOperation = SceneManager.LoadSceneAsync("Level_" + level, LoadSceneMode.Additive);
        levelLoadOperation.allowSceneActivation = false;
        Debug.Log("Started loading Level_" + level);

        Image loadingImage = GameObject.FindGameObjectWithTag("loding").GetComponent<Image>();
        if (loadingImage == null)
        {
            Debug.LogError("Loading image not found!");
            yield break;
        }

        float timer = 0f;
        while (mainLoadOperation.progress < 0.9f || levelLoadOperation.progress < 0.9f)
        {
            yield return null;
            float progress = Mathf.Min(mainLoadOperation.progress, levelLoadOperation.progress);
            loadingImage.fillAmount = Mathf.Lerp(0, 0.9f, progress);
            Debug.Log($"Loading progress - Main: {mainLoadOperation.progress}, Level: {levelLoadOperation.progress}");
        }

        Debug.Log("Both scenes reached 90% loading");
        while (loadingImage.fillAmount < 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            loadingImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
        }

        Debug.Log("Activating scenes");
        mainLoadOperation.allowSceneActivation = true;
        levelLoadOperation.allowSceneActivation = true;

        GameManager.Instance.SaveLevel(level);
        _currentLevel = level;
        GameManager.Instance.isLoding = false;
        Debug.Log("Scenes activated and loading complete");
    }
    
    // private IEnumerator LoadMainAndLevelCoroutine(int level)
    // {
    //     GameManager.Instance.isLoding = true;
    //     // 로딩 씬 로드
    //     yield return SceneManager.LoadSceneAsync("SceneLoad", LoadSceneMode.Single);
    //
    //     // main 씬 로드
    //     AsyncOperation mainLoadOperation = SceneManager.LoadSceneAsync("main", LoadSceneMode.Single);
    //     mainLoadOperation.allowSceneActivation = false;
    //
    //     float timer = 0f;
    //     Image loadingImage = GameObject.FindGameObjectWithTag("loding").GetComponent<Image>();
    //
    //     // main 씬 0.9까지 로드
    //     while (mainLoadOperation.progress < 0.9f)
    //     {
    //         yield return null;
    //         loadingImage.fillAmount = mainLoadOperation.progress/2;
    //         //Debug.Log(mainLoadOperation.progress);
    //     }
    //
    //     // level 씬 로드 시작
    //     AsyncOperation levelLoadOperation = SceneManager.LoadSceneAsync("Level_" + level, LoadSceneMode.Additive);
    //     levelLoadOperation.allowSceneActivation = false;
    //
    //     // level 씬 로드 완료 대기
    //     while (!levelLoadOperation.isDone)
    //     {
    //         yield return null;
    //     
    //         timer += Time.deltaTime;
    //         loadingImage.fillAmount = Mathf.Lerp(0.5f, 1f, timer);
    //     
    //         if (loadingImage.fillAmount >= 1f)
    //         {
    //             mainLoadOperation.allowSceneActivation = true;
    //             levelLoadOperation.allowSceneActivation = true;
    //         }
    //         
    //         //Debug.Log(levelLoadOperation.progress);
    //     }
    //
    //     GameManager.Instance.SaveLevel(level);
    //     _currentLevel = level;
    //
    //     //Debug.Log("Main and Level scenes loaded");
    //     GameManager.Instance.isLoding = false;
    // }

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
    
    //씬 전환
    public void CheckChangeScene(Collider2D other, int num)
    {
        num += 1;
        if ((other.CompareTag("Body") ) && !GameManager.Instance.CheckLoadScene("Level_"+num) &&
            (GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.IsCombine)))
        {
            GameManager.Instance.LoadMainAndLevel(num);
        }

        if (num != 2 && num != 3)
        {
            if(other.CompareTag("Arrow")||((other.CompareTag("Body") || other.CompareTag("Head")) && !GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.IsCombine)))
            {
                GameManager.Instance.LoadMainAndLevel(PlayerPrefs.GetInt("level"));
                GameManager.Instance.isPaused = false;
                GameManager.Instance.isEscMenuView = false;
                Time.timeScale = 1;
            }
        }
        
    }
    
}