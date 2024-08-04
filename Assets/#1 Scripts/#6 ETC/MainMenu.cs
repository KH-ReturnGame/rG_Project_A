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
            GameManager.inst.ChangeLevel(1,LoadSceneMode.Single);
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
            EasyButton.SetActive(true);
            HardButton.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(0.025f);
            EasyButton.SetActive(false);
            HardButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (StartButtonToggle)
        {
            startRectT.anchoredPosition3D =
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(700, 100, 0), 10f*Time.deltaTime);

            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(700, -10, 0), 10f*Time.deltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(700, -110, 0), 10f*Time.deltaTime);

        }
        else
        {
            startRectT.anchoredPosition3D =
                Vector3.Slerp(startRectT.anchoredPosition3D, new Vector3(700, 0, 0), 10f*Time.deltaTime);
            
            easyRectT.anchoredPosition3D =
                Vector3.Slerp(easyRectT.anchoredPosition3D, new Vector3(700, 90, 0), 10f*Time.deltaTime);
            
            hardRectT.anchoredPosition3D =
                Vector3.Slerp(hardRectT.anchoredPosition3D, new Vector3(700, -10, 0), 10f*Time.deltaTime);
        }
    }
}
