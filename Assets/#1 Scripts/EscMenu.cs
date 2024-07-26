using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EscMenu : MonoBehaviour
{
    public Button ExitButton;
    public Button SettingButton;
    public Button ExitgameButton;

    private void Start()
    {
        ExitButton.onClick.AddListener(() => { GameManager.inst.UnLoadScene(Scenes.EscMenu);});
        SettingButton.onClick.AddListener(() => { GameManager.inst.ChangeScene(Scenes.Setting, LoadSceneMode.Additive);});
        ExitgameButton.onClick.AddListener(() => { Application.Quit();});
    }
}