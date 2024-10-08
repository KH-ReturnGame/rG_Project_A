using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EscMenu : MonoBehaviour
{
    public Button SettingButton;
    public Button MainmenuButton;
    public Button ExitgameButton;

    private void Start()
    {
        SettingButton.onClick.AddListener(() => { GameManager.Instance.ToggleSettingMenu(); });
        MainmenuButton.onClick.AddListener(() => { 
            GameManager.Instance.ChangeScene(Scenes.MainMenu, LoadSceneMode.Single);
            GameManager.Instance.isEscMenuView = false;
            GameManager.Instance.isPaused = false;
            Time.timeScale = 1;
        });
        ExitgameButton.onClick.AddListener(() => { Application.Quit();});
    }
}