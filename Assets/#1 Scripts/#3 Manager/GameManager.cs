using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private SceneLoader SL;
    private SignalManager SM;

    public GameObject EscMenuObj;
    public bool isEscMenuView;
    private GameObject CreatedEscMenu;

    public GameObject SettingMenuObj;
    public bool isSettingMenuView;
    private GameObject CreatedSettingMenu;

    public bool isPaused = false;
    
    public void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (inst != this)
        {
            Destroy(gameObject);
        }
        SL = GetComponent<SceneLoader>();
        SM = GetComponent<SignalManager>();
    }
    public static GameManager Instance
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<GameManager>();
                if (inst == null)
                {
                    GameObject go = new GameObject("GameManager");
                    inst = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return inst;
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // GameManager 초기화 또는 참조 업데이트
        if (GameManager.Instance != null)
        {
            Debug.Log("GameManager instance is available after scene load");
        }
        else
        {
            Debug.LogWarning("GameManager instance is null after scene load");
        }
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingMenuView)
        {
            isEscMenuView = !isEscMenuView;
            if (isEscMenuView)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
        CreatedEscMenu = Instantiate(EscMenuObj, canvas.transform, false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Destroy(CreatedEscMenu);
    }

    public void ToggleSettingMenu()
    {
        isSettingMenuView = !isSettingMenuView;
        if (isSettingMenuView)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            CreatedSettingMenu = Instantiate(SettingMenuObj, canvas.transform, false);
        }
        else
        {
            Destroy(CreatedSettingMenu);
        }
    }

    //Scene 관리
    public void ChangeLevel(int l,LoadSceneMode mode)
    {
        SL.ChangeLevel(l,mode);
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SL.ChangeScene(scene,mode);
    }

    public void UnLoadScene(Scenes scene)
    {
        SL.UnLoadScene(scene);
    }

    public bool CheckLoadScene(string scene)
    {
        return SL.CheckLoadScene(scene);
    }

    public bool CheckLoadScene(Scenes scene)
    {
        return SL.CheckLoadScene(scene);
    }

    //카메라 관리
    public void ChangeCameraTarget(GameObject obj)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ChangeTarget(obj);
    }
    
    //신호 관리
    public bool CheckSignal(int index)
    {
        return SM.CheckSignal(index);
    }
    public void ChangeSignal(int index, bool signal)
    {
        SM.ChangeSignal(index, signal);
    }
    public void AddSendObj(GameObject obj, int index)
    {
        //Debug.Log(obj.name);
        SM.AddSendObj(obj,index);
    }
    public void RemSendObj(GameObject obj)
    {
        SM.RemSendObj(obj);
    }
    
}
