using System;
using UnityEngine;

public enum BodyStates
{
    IsGround=0,
}

public class Body : MonoBehaviour
{
    //몸이 가질 수 있는 모든 상태 개수
    public static int state_count = Enum.GetValues(typeof(BodyStates)).Length;
    //몸이 가질 수 있는 모든 상태들 배열
    public State<Body>[] _states;
    public StateManager<Body> _stateManager;
    
    //기본 설정
    public void Start()
    {
        //_states 초기화
        _states = new State<Body>[state_count];
        _states[(int)BodyStates.IsGround] = new BodyOwnedStates.IsGround();

        _stateManager = new StateManager<Body>();
        _stateManager.Setup(this,state_count,_states);
    }
    
    //부모의 추상 메소드를 구현, Entity_Manager의 Update에서 반복함
    public void Update()
    {
        //상태 매니저의 Execute실행
        _stateManager.Execute();
    }

    //상태 추가 메소드
    public void AddState(BodyStates bs)
    {
        State<Body> newState = _states[(int)bs];
        _stateManager.AddState(newState);
    }
    
    //상태 제거 메소드
    public void RemoveState(BodyStates bs)
    {
        State<Body> remState = _states[(int)bs];
        _stateManager.RemoveState(remState);
    }
    //상태 있는지 체크
    public bool IsContainState(BodyStates bs)
    {
        return _stateManager._currentState.Contains(_states[(int)bs]);
    }
}
