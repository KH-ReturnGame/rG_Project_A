using System;
using UnityEngine;

public enum PlayerStats
{
    BodyIsGround=0,
    HeadIsGround,
}

public class Player : MonoBehaviour
{
    //플레이어가 가질 수 있는 모든 상태 개수
    private static readonly int StateCount = Enum.GetValues(typeof(PlayerStats)).Length;
    //플레이어가 가질 수 있는 모든 상태들 배열
    private State<Player>[] _states;
    private StateManager<Player> _stateManager;
    
    //기본 설정
    public void Start()
    {
        //_states 초기화
        _states = new State<Player>[StateCount];
        _states[(int)PlayerStats.BodyIsGround] = new PlayerOwnedStates.BodyIsGround();
        _states[(int)PlayerStats.HeadIsGround] = new PlayerOwnedStates.HeadIsGround();
        

        _stateManager = new StateManager<Player>();
        _stateManager.Setup(this,StateCount,_states);
    }
    
    //업데이트 메소드
    public void Update()
    {
        //상태 매니저의 Execute실행
        _stateManager.Execute();
    }

    //상태 추가 메소드
    public void AddState(PlayerStats bs)
    {
        State<Player> newState = _states[(int)bs];
        _stateManager.AddState(newState);
    }
    
    //상태 제거 메소드
    public void RemoveState(PlayerStats bs)
    {
        State<Player> remState = _states[(int)bs];
        _stateManager.RemoveState(remState);
    }
    //상태 있는지 체크
    public bool IsContainState(PlayerStats bs)
    {
        return _stateManager._currentState.Contains(_states[(int)bs]);
    }
}