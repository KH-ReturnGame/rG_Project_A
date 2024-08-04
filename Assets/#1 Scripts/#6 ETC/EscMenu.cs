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
        SettingButton.onClick.AddListener(() => { GameManager.inst.ToggleSettingMenu(); });
        MainmenuButton.onClick.AddListener(() => { 
            GameManager.inst.ChangeScene(Scenes.MainMenu, LoadSceneMode.Single);
            GameManager.inst.isEscMenuView = false;
            GameManager.inst.isPaused = false;
            Time.timeScale = 1;
        });
        ExitgameButton.onClick.AddListener(() => { Application.Quit();});
    }
}