using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Body") || other.CompareTag("Head")) && !GameManager.inst.CheckLoadScene("Level_2"))
        {
            GameManager.inst.ChangeLevel(2,LoadSceneMode.Additive);
        }
    }
}
