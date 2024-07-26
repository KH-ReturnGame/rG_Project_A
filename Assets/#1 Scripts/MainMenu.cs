using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button TutorialButton;
    public Button SettingButton;

    private void Start()
    {
        StartButton.onClick.AddListener(() => { GameManager.inst.ChangeLevel(1, LoadSceneMode.Single);});
        TutorialButton.onClick.AddListener(() => { GameManager.inst.ChangeLevel(0, LoadSceneMode.Single);});
        SettingButton.onClick.AddListener(() => { GameManager.inst.ChangeScene(Scenes.Setting, LoadSceneMode.Single);});
    }
}
