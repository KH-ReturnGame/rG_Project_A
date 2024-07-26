using UnityEngine.SceneManagement;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    //현재 레벨 , 0 = 튜토리얼 , n ~~~ = n레벨
    private int currentLevel = 0;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ChangeLevel(int l)
    {
        string level = "Level_" + l.ToString();
        SceneManager.LoadScene(level);
        currentLevel = l;
    }
}
