using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_6_Manager : MonoBehaviour
{
    public Transform head;
    public Transform body;
    public Transform arrow;
    public Transform cam;
    public void Awake()
    {
        GameManager.Instance.ResetPlayer(head,body,arrow,cam,true,true,false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Body") ) && !GameManager.Instance.CheckLoadScene("EndingScene") &&
            (GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.IsCombine)))
        {
            GameManager.Instance.ChangeScene(Scenes.EndingScene,LoadSceneMode.Single);
        }

        if(other.CompareTag("Arrow")||((other.CompareTag("Body") || other.CompareTag("Head")) && !GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.IsCombine)))
        {
            GameManager.Instance.LoadMainAndLevel(PlayerPrefs.GetInt("level"));
            GameManager.Instance.isPaused = false;
            GameManager.Instance.isEscMenuView = false;
            Time.timeScale = 1;
        }
    }
}
