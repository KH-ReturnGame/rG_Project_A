using UnityEngine.SceneManagement;
using UnityEngine;

public enum Scenes
{
    MainMenu,
    Setting,
}

public class SceneLoader : MonoBehaviour
{
    //현재 레벨 , -1 = 설정 , 0 = 튜토리얼 , n ~~~ = n레벨
    private int _currentLevel = 0;

    //모든 씬을 넘나들며 관리를 해줘야 하기 때문에 씬 로드시 삭제하지 않도록 설정
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //씬 중에서도 레벨씬 전환
    public void ChangeLevel(int l, LoadSceneMode mode)
    {
        string level = "Level_" + l;
        SceneManager.LoadScene(level,mode);
        _currentLevel = l;
    }

    //레벨 이외의 씬을 변경할때 사용
    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SceneManager.LoadScene(scene.ToString(), mode);
    }
    
}