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

    public void ChangeLevel(int l, LoadSceneMode mode)
    {
        SL.ChangeLevel(l,mode);
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SL.ChangeScene(scene,mode);
    }
}
