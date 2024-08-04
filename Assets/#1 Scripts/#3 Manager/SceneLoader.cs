using UnityEngine.SceneManagement;
using UnityEngine;

public enum Scenes
{
    MainMenu,
    Setting,
    EscMenu,
}

public class SceneLoader : MonoBehaviour
{
    //현재 레벨 , n ~~~ = n레벨
    private int _currentLevel = 1;

    //씬 중에서도 레벨씬 전환
    public void ChangeLevel(int l,LoadSceneMode mode)
    {
        string level = "Level_" + l;
        SceneManager.LoadScene(level,mode);
        _currentLevel = l;
    }

    //레벨 이외의 씬을 변경할때 사용
    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SceneManager.LoadScene(scene.ToString(), mode);
        if (mode == 0)
        {
            Debug.Log(SceneManager.GetActiveScene());
        }
    }

    public void UnLoadScene(Scenes scene)
    {
        SceneManager.UnloadSceneAsync(scene.ToString());
    }

    public bool CheckLoadScene(string scene)
    {
        return SceneManager.GetSceneByName(scene).isLoaded;
    }
    public bool CheckLoadScene(Scenes scene)
    {
        return SceneManager.GetSceneByName(scene.ToString()).isLoaded;
    }
    
}