using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Instance 정적 변수
    public static GameManager inst;
    
    //씬 로더 & 시그널 매니저
    private SceneLoader SL;
    private SignalManager SM;

    //Esc 메뉴
    public GameObject EscMenuObj;
    public bool isEscMenuView;
    private GameObject CreatedEscMenu;

    //Setting 메뉴
    public GameObject SettingMenuObj;
    public bool isSettingMenuView;
    private GameObject CreatedSettingMenu;

    //게임 일시정지
    public bool isPaused = false;
    public bool hasFocus;
    
    //인스턴스화 되었을때
    public void Awake()
    {
        //아무런 인스턴스가 없으면
        if (inst == null)
        {
            //자기 자신 인스턴스로 ㄱㄱ
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        //인스턴스가 있으면?
        else if (inst != this)
        {
            //나 지우기 ㅠㅠ
            Destroy(gameObject);
        }
        
        //기타 매니저들 가져오기
        SL = GetComponent<SceneLoader>();
        SM = GetComponent<SignalManager>();
    }
    
    //게임 매니저 속성으로 접근과 관련된 규약 지정하는 부분
    public static GameManager Instance
    {
        //게임 매니저를 호출했을때 (ex GameManager.~~)
        get
        {
            //인스턴스가 없어??? --> Awake에서 항상 없으면 만들기 때문에 여기가 호출될 일은 없을듯?
            if (inst == null)
            {
                //게임 매니저 찾아서 넣기
                inst = FindObjectOfType<GameManager>();
                //근데 못찾았어???
                if (inst == null)
                {
                    //하나 만들어~
                    GameObject go = new GameObject("GameManager");
                    inst = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            
            //인스턴스 반환
            return inst;
        }
    }
    
    //게임매니저가 활성화 되었을때
    private void OnEnable()
    {
        //씬 로드 이벤트에 OnSceneLoaded 핸들로 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //게임매니저가 비활성화 되었을때
    private void OnDisable()
    {
        //씬 로드 이벤트에 OnSceneLoaded 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //씬 로드될때 실행되는 이벤트
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // GameManager 초기화 또는 참조 업데이트
        if (GameManager.Instance != null)
        {
            Debug.Log("GameManager instance is available after scene load");
        }
        else
        {
            Debug.LogWarning("GameManager instance is null after scene load");
        }
    }
    
    //유니티 일시정지 이벤트
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PauseGame();
        }
        else if (!isEscMenuView)
        {
            ResumeGame();
        }
    }
    
    //유니티 다른 화면 넘어갔을때
    private void OnApplicationFocus(bool focus)
    {
        hasFocus = focus;
    }
    
    //게임 일시정지
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    //게임 재개
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
    
    //Esc 메뉴 무한 체크
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingMenuView)
        {
            isEscMenuView = !isEscMenuView;
            if (isEscMenuView)
            {
                PauseGame();
                GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
                CreatedEscMenu = Instantiate(EscMenuObj, canvas.transform, false);
            }
            else
            {
                ResumeGame();
                Destroy(CreatedEscMenu);
            }
        }
    }

    //설정 메뉴 열고 끄기
    public void ToggleSettingMenu()
    {
        isSettingMenuView = !isSettingMenuView;
        if (isSettingMenuView)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("canvas");
            CreatedSettingMenu = Instantiate(SettingMenuObj, canvas.transform, false);
        }
        else
        {
            Destroy(CreatedSettingMenu);
        }
    }

    //Scene 관리
    public void ChangeLevel(int l,LoadSceneMode mode)
    {
        SL.ChangeLevel(l,mode);
    }

    public void ChangeScene(Scenes scene, LoadSceneMode mode)
    {
        SL.ChangeScene(scene,mode);
    }

    public void UnLoadScene(Scenes scene)
    {
        SL.UnLoadScene(scene);
    }

    public bool CheckLoadScene(string scene)
    {
        return SL.CheckLoadScene(scene);
    }

    public bool CheckLoadScene(Scenes scene)
    {
        return SL.CheckLoadScene(scene);
    }

    //카메라 관리
    public void ChangeCameraTarget(GameObject obj)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().ChangeTarget(obj);
    }
    
    //신호 관리
    public bool CheckSignal(int index)
    {
        return SM.CheckSignal(index);
    }
    public void ChangeSignal(int index, bool signal)
    {
        SM.ChangeSignal(index, signal);
    }
    public void AddSendObj(GameObject obj, int index)
    {
        //Debug.Log(obj.name);
        SM.AddSendObj(obj,index);
    }
    public void RemSendObj(GameObject obj)
    {
        SM.RemSendObj(obj);
    }
    
}
