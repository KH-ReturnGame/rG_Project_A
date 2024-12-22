using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

public class SettingManager : MonoBehaviour
{
    // 해상도 설정
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> resolutions = new List<Resolution>();
    private int optimalResolutionIndex = 0;
    private Resolution currentResolution; // 현재 해상도를 저장하는 변수
    //public PixelPerfectCamera pixelPerfectCamera;

    // 전체화면 모드 설정
    public TMP_Dropdown fullscreenDropdown;
    private List<string> fullscreenModes = new List<string> { "Windowed", "ExclusiveFullScreen", "FullScreenWindow" };

    // 소리 슬라이더
    public Slider bgm_slider;
    public Slider sound_slider;

    // 세팅 메뉴 종료
    public void ExitSetting()
    {
        GameManager.Instance.ToggleSettingMenu();
    }

    // Start
    private void Start()
    {
       // pixelPerfectCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PixelPerfectCamera>();
        
        // 해상도 설정 리스트 추가
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1600, height = 900 });
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 3840, height = 2160 });

        // 현재 해상도 저장
        currentResolution = Screen.currentResolution;
        
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

        // 해상도 및 전체화면 모드 로드
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
        LoadAudioSettings(); // 오디오 설정 로드

        // 이벤트 리스너 등록
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreenMode);
        bgm_slider.onValueChanged.AddListener(ChangeBgmSlider);
        sound_slider.onValueChanged.AddListener(ChangeSoundSlider);
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
        FullScreenMode currentMode = Screen.fullScreenMode;
        int savedFullscreenMode = PlayerPrefs.GetInt("FullscreenMode", (int)currentMode);
        fullscreenDropdown.value = savedFullscreenMode;
        fullscreenDropdown.RefreshShownValue();
        //SetFullscreenMode(savedFullscreenMode);
    }

    // 해상도 설정
    // public void SetResolution(int resolutionIndex)
    // {
    //     // 현재 해상도와 선택된 해상도를 비교하여 다를 경우에만 설정 변경
    //     Resolution selectedResolution = resolutions[resolutionIndex];
    //     if (selectedResolution.width != currentResolution.width || selectedResolution.height != currentResolution.height)
    //     {
    //         PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    //         Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    //         currentResolution = selectedResolution; // 새로운 해상도를 현재 해상도로 저장
    //     }
    // }
    //
    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = resolutions[resolutionIndex];
        if (selectedResolution.width != currentResolution.width || selectedResolution.height != currentResolution.height)
        {
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            currentResolution = selectedResolution; // 새로운 해상도를 현재 해상도로 저장

            // // Pixel Perfect Camera의 ReferenceResolution 업데이트
            // if (pixelPerfectCamera != null)
            // {
            //     pixelPerfectCamera.refResolutionX = selectedResolution.width;
            //     pixelPerfectCamera.refResolutionY = selectedResolution.height;
            //     
            //     // Pixel Perfect Camera 업데이트 강제 반영
            //     pixelPerfectCamera.enabled = false; // 일단 비활성화
            //     pixelPerfectCamera.enabled = true;  // 다시 활성화
            //     
            // }
        }
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

    // BGM 슬라이더 값 변경
    public void ChangeBgmSlider(float value)
    {
        PlayerPrefs.SetFloat("BgmVolume", value); // 값 저장
        GameObject[] audios = GameObject.FindGameObjectsWithTag("bgm");
        foreach (var audio in audios)
        {
            audio.GetComponent<AudioSource>().volume = 0.4f * value;
        }
    }

    // 사운드 슬라이더 값 변경
    public void ChangeSoundSlider(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value); // 값 저장
        GameObject[] audios = GameObject.FindGameObjectsWithTag("sound");
        foreach (var audio in audios)
        {
            audio.GetComponent<AudioSource>().volume = value;
        }
    }

    // 오디오 설정 로드
    public void LoadAudioSettings()
    {
        float savedBgmVolume = PlayerPrefs.GetFloat("BgmVolume", 1f); // 기본값 1
        float savedSoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f); // 기본값 1

        bgm_slider.value = savedBgmVolume;
        sound_slider.value = savedSoundVolume;

        ChangeBgmSlider(savedBgmVolume); // 로드된 값 적용
        ChangeSoundSlider(savedSoundVolume); // 로드된 값 적용
    }
}
