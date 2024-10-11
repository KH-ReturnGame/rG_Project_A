using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
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
        if ((other.CompareTag("Body") || other.CompareTag("Head") || other.CompareTag("Arrow")) && !GameManager.Instance.CheckLoadScene("Level_2") &&
            (GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.IsCombine)))
        {
            GameManager.Instance.LoadMainAndLevel(2);
        }
    }
}
