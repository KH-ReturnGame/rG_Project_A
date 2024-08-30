using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_2_Manager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Body") || other.CompareTag("Head") || other.CompareTag("Arrow")) && !GameManager.Instance.CheckLoadScene("Level_3"))
        {
            GameManager.Instance.ChangeLevel(3,LoadSceneMode.Additive);
        }
    }
}
