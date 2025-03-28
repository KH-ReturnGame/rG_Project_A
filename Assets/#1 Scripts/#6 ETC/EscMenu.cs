using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class EscMenu : MonoBehaviour
{
    public Button SettingButton;
    public Button MainmenuButton;
    public Button RestartButton;
    public Button ExitgameButton;
    
    private RectTransform settingRectT;
    private RectTransform mainmenuRectT;
    private RectTransform restartRectT;
    private RectTransform exitgameRectT;

    private void Start()
    {
        SettingButton.onClick.AddListener(() => { GameManager.Instance.ToggleSettingMenu(); });
        MainmenuButton.onClick.AddListener(() => { 
            GameManager.Instance.ChangeScene(Scenes.MainMenu, LoadSceneMode.Single);
            GameManager.Instance.isEscMenuView = false;
            GameManager.Instance.isPaused = false;
            GameManager.inst.isPlayGame = false;
            Time.timeScale = 1;
            GameManager.Instance.isSpeedRun = false;
        });
        RestartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadMainAndLevel(PlayerPrefs.GetInt("level"));
            GameManager.Instance.isPaused = false;
            GameManager.Instance.isEscMenuView = false;
            Time.timeScale = 1;
        });
        ExitgameButton.onClick.AddListener(() => { Application.Quit();});
        
        settingRectT = SettingButton.GetComponent<RectTransform>();
        mainmenuRectT = MainmenuButton.GetComponent<RectTransform>();
        restartRectT = RestartButton.GetComponent<RectTransform>();
        exitgameRectT = ExitgameButton.GetComponent<RectTransform>();
        
        if (GameManager.Instance.isPlayGame)
        {
            settingRectT.anchoredPosition3D = new Vector3(0, 210, 0);
            mainmenuRectT.anchoredPosition3D = new Vector3(0, 70, 0);
            restartRectT.anchoredPosition3D = new Vector3(0, -70, 0);
            exitgameRectT.anchoredPosition3D = new Vector3(0, -210, 0);
        }
        else
        {
            settingRectT.anchoredPosition3D = new Vector3(0, 70, 0);
            MainmenuButton.gameObject.SetActive(false);
            RestartButton.gameObject.SetActive(false);
            exitgameRectT.anchoredPosition3D = new Vector3(0, -70, 0);
        }
    }

    public void Update()
    {
        // if (GameManager.Instance.isPlayGame)
        // {
        //     settingRectT.anchoredPosition3D = new Vector3(0, 0, 210);
        //     mainmenuRectT.anchoredPosition3D = new Vector3(0, 0, 70);
        //     restartRectT.anchoredPosition3D = new Vector3(0, 0, -70);
        //     exitgameRectT.anchoredPosition3D = new Vector3(0, 0, -210);
        // }
        // else
        // {
        //     settingRectT.anchoredPosition3D = new Vector3(0, 0, 140);
        //     mainmenuRectT.anchoredPosition3D = new Vector3(0, 0, 0);
        //     ReStartButton.gameObject.SetActive(false);
        //     exitgameRectT.anchoredPosition3D = new Vector3(0, 0, --140);
        // }
    }
}