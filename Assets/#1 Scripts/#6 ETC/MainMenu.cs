using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject EasyButton;
    public GameObject HardButton;

    private RectTransform startRectT;
    private RectTransform easyRectT;
    private RectTransform hardRectT;

    private bool StartButtonToggle = false;
    
    
    private void Start()
    {
        StartButton.GetComponent<Button>().onClick.AddListener(ClickStartButton);
        EasyButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            int level = PlayerPrefs.HasKey("level") ? PlayerPrefs.GetInt("level") : 1;
            GameManager.Instance.LoadMainAndLevel(level);
        });
        HardButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            //뭔가 타이머 시작을 해야함
            PlayerPrefs.SetInt("level",1);
            GameManager.Instance.LoadMainAndLevel(1);
            GameManager.Instance.isSpeedRun = true;
            GameManager.Instance.totalTime = 0f;
        });

        startRectT = StartButton.GetComponent<RectTransform>();
        easyRectT = EasyButton.GetComponent<RectTransform>();
        hardRectT = HardButton.GetComponent<RectTransform>();
    }

    private void ClickStartButton()
    {
        StartButtonToggle = !StartButtonToggle;
        if (StartButtonToggle)
        {
            StartCoroutine("Active");
        }
        else
        {
            StartCoroutine("Active");
        }
    }

    public void ClickSettingButton()
    {
        GameManager.Instance.ToggleSettingMenu();
    }

    public void ClickExitBtn()
    {
        Application.Quit();
    }

    IEnumerator Active()
    {
        if (StartButtonToggle)
        {
            yield return new WaitForSeconds(0.15f);
            if (!EasyButton.activeSelf && !HardButton.activeSelf)
            {
                EasyButton.SetActive(true);
                HardButton.SetActive(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.025f);
            if (EasyButton.activeSelf && HardButton.activeSelf)
            {
                EasyButton.SetActive(false);
                HardButton.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (StartButtonToggle)
        {
            startRectT.anchoredPosition3D =
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(670, 100, 0), 10f*Time.unscaledDeltaTime);

            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(670, -10, 0), 10f*Time.unscaledDeltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(670, -110, 0), 10f*Time.unscaledDeltaTime);

        }
        else
        {
            startRectT.anchoredPosition3D =
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(670, 0, 0), 10f*Time.unscaledDeltaTime);
            
            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(670, 90, 0), 10f*Time.unscaledDeltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(670, -10, 0), 10f*Time.unscaledDeltaTime);
        }
    }
}
