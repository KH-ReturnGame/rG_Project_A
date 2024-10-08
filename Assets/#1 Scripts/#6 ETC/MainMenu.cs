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
            GameManager.Instance.ChangeLevel(1,LoadSceneMode.Single);
        });
        HardButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            
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
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(840, 100, 0), 10f*Time.unscaledDeltaTime);

            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(840, -10, 0), 10f*Time.unscaledDeltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(840, -110, 0), 10f*Time.unscaledDeltaTime);

        }
        else
        {
            startRectT.anchoredPosition3D =
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(840, 0, 0), 10f*Time.unscaledDeltaTime);
            
            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(840, 90, 0), 10f*Time.unscaledDeltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(840, -10, 0), 10f*Time.unscaledDeltaTime);
        }
    }
}
