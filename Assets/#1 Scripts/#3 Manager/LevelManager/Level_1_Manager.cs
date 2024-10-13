using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
{
    public Transform head;
    public Transform body;
    public Transform arrow;
    public Transform cam;

    public GameObject[] Comments;
    private int index = 0;
    
    public void Awake()
    {
        GameManager.Instance.ResetPlayer(head,body,arrow,cam,true,true,false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.CheckChangeScene(other,1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && index < 3)
        {
            Comments[index].SetActive(false);
            index += 1;
            if (index == 3)
            {
                return;
            }
            Comments[index].SetActive(true);
        }
    }
}
