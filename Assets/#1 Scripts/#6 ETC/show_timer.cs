using System;
using TMPro;
using UnityEngine;

public class show_timer : MonoBehaviour
{
    public GameObject timer;
    private void Update()
    {
        if (GameManager.Instance.isSpeedRun)
        {
            timer.SetActive(true);
            timer.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.totalTime.ToString("F2") + "s";
        }
    }
}