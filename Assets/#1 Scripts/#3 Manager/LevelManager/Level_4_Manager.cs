using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_4_Manager : MonoBehaviour
{
    public Transform head;
    public Transform body;
    public Transform arrow;
    public void Awake()
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.GetPlayerObj(PlayerObj.Head).GetComponent<Transform>().position = head.position;
        player.GetPlayerObj(PlayerObj.Body).GetComponent<Transform>().position = body.position;
        player.GetPlayerObj(PlayerObj.Arrow).GetComponent<Transform>().position = arrow.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Body") || other.CompareTag("Head") || other.CompareTag("Arrow")) && !GameManager.Instance.CheckLoadScene("Level_5"))
        {
            GameManager.Instance.ChangeLevel(5,LoadSceneMode.Additive);
        }
    }
}
