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
                GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
                CreatedEscMenu = Instantiate(EscMenuObj, canvas.transform, false);
            }
            else
            {
                Destroy(CreatedEscMenu);
            }
        }
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

    public void ChangeLevel(int l)
    {
        SL.ChangeLevel(l);
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SL.ChangeScene(scene,mode);
    }

    public void UnLoadScene(Scenes scene)
    {
        SL.UnLoadScene(scene);
    }
}
