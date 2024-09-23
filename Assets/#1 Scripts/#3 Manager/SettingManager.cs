using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    // 해상도 설정
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> resolutions = new List<Resolution>();
    private int optimalResolutionIndex = 0;

    // 전체화면 모드 설정
    public TMP_Dropdown fullscreenDropdown;
    private List<string> fullscreenModes = new List<string> { "창모드", "전체화면", "전체화면 창모드" };

    // 세팅 메뉴 종료
    public void ExitSetting()
    {
        GameManager.Instance.ToggleSettingMenu();
    }

    // Start
    private void Start()
    {
        // 해상도 설정 리스트 추가
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1280, height = 800 });
        resolutions.Add(new Resolution { width = 1440, height = 900 });
        resolutions.Add(new Resolution { width = 1600, height = 900 });
        resolutions.Add(new Resolution { width = 1680, height = 1050 });
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 1920, height = 1200 });
        resolutions.Add(new Resolution { width = 2048, height = 1280 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 2560, height = 1600 });
        resolutions.Add(new Resolution { width = 2880, height = 1800 });
        resolutions.Add(new Resolution { width = 3480, height = 2160 });

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                optimalResolutionIndex = i;
                option += " *";
            }
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    
        fullscreenDropdown.AddOptions(fullscreenModes);

        // 해상도 및 전체화면 모드 로드 (이미 설정된 값이 있는지 확인)
        if (!PlayerPrefs.HasKey("ResolutionIndex"))
        {
            PlayerPrefs.SetInt("ResolutionIndex", optimalResolutionIndex);
            SetResolution(optimalResolutionIndex); // 처음 실행될 때만 설정
        }
        else
        {
            LoadResolution(); // 설정된 값이 있으면 로드
        }

        LoadFullscreenMode();

        // 드롭다운 이벤트 리스너 등록
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreenMode);
    }
    // 해상도 설정 로드
    private void LoadResolution()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", optimalResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedResolutionIndex);
    }

    // 전체화면 모드 설정 로드
    private void LoadFullscreenMode()
    {
        // 현재 화면 모드 가져오기
        FullScreenMode currentMode = Screen.fullScreenMode;
        int savedFullscreenMode = PlayerPrefs.GetInt("FullscreenMode", (int)currentMode);
        fullscreenDropdown.value = savedFullscreenMode;
        fullscreenDropdown.RefreshShownValue();
        SetFullscreenMode(savedFullscreenMode);
    }

    // 해상도 설정
    public void SetResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // 전체화면 모드 설정
    public void SetFullscreenMode(int modeIndex)
    {
        PlayerPrefs.SetInt("FullscreenMode", modeIndex);
        switch (modeIndex)
        {
            case 0: // 창모드
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // 전체화면
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2: // 전체화면 창모드
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }
}