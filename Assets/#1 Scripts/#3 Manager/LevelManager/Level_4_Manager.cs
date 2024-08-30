using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_4_Manager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Body") || other.CompareTag("Head") || other.CompareTag("Arrow")) && !GameManager.Instance.CheckLoadScene("Level_5"))
        {
            GameManager.Instance.ChangeLevel(5,LoadSceneMode.Additive);
        }
    }
}
