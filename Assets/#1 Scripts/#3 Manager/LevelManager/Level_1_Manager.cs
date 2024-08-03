using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
{
    private bool isLoad = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Body")&&!isLoad) || (other.CompareTag("Head")&&!isLoad))
        {
            isLoad = true;
            GameManager.inst.ChangeLevel(2,LoadSceneMode.Additive);
        }
    }
}
