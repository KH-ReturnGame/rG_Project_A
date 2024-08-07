using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Body") || other.CompareTag("Head")) && !GameManager.Instance.CheckLoadScene("Level_2"))
        {
            GameManager.Instance.ChangeLevel(2,LoadSceneMode.Additive);
        }
    }
}
