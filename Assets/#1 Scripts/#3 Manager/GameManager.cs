using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private SceneLoader SL;
    
    public void Awake()
    {
        inst = this;
        DontDestroyOnLoad(this);
        SL = GetComponent<SceneLoader>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SL.CheckLoadScene(Scenes.Setting))
        {
            if (SL.CheckLoadScene(Scenes.EscMenu))
            {
                UnLoadScene(Scenes.EscMenu);
            }
            else
            {
                ChangeScene(Scenes.EscMenu,LoadSceneMode.Additive);
            }
        }
    }

    public void ChangeLevel(int l)
    {
        SL.ChangeLevel(l);
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SL.ChangeScene(scene,mode);
    }

    public void UnLoadScene(Scenes scene)
    {
        SL.UnLoadScene(scene);
    }
}
