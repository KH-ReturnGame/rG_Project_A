using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManageer : MonoBehaviour
{
    
    public TMP_Dropdown resolutionDropdown;
 
    private List<Resolution> resolutions = new List<Resolution>();
    private int optimalResolutionIndex = 0;
    
    public void ExitSetting()
    {
        GameManager.Instance.ToggleSettingMenu();
    }
 
    private void Start()
    {
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
            // 가장 적합한 해상도에 별표를 표기합니다.
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                optimalResolutionIndex = i;
                option += " *";
            }
            options.Add(option);
        }
 
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = optimalResolutionIndex;
        resolutionDropdown.RefreshShownValue();
 
        // 게임이 가장 적합한 해상도로 시작되도록 설정합니다.
        SetResolution(optimalResolutionIndex);
    }
 
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
