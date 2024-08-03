using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private SceneLoader SL;

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
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        SL = GetComponent<SceneLoader>();
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

    public void ChangeCameraTarget(GameObject obj)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ChangeTarget(obj);
    }
}
