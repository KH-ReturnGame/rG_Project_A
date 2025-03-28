using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_7_Manager : MonoBehaviour
{
    public Transform head;
    public Transform body;
    public Transform arrow;
    public Transform cam;
    public void Awake()
    {
        GameManager.Instance.ResetPlayer(head,body,arrow,cam,true,true,true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.CheckChangeScene(other,7);
    }
}
