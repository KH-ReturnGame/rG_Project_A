using System;
using UnityEngine;

public enum HeadStates
{
    IsGround,
}

public class Head : MonoBehaviour
{
    //몸이 가질 수 있는 모든 상태 개수
    public static int state_count = Enum.GetValues(typeof(HeadStates)).Length;
    //몸이 가질 수 있는 모든 상태들 배열
    public State<Head>[] _states;
    public StateManager<Head> _stateManager;
    
    //기본 설정
    public void Start()
    {
        //_states 초기화
        _states = new State<Head>[state_count];
        _states[(int)BodyStates.IsGround] = new HeadOwnedStates.IsGround();

        _stateManager = new StateManager<Head>();
        _stateManager.Setup(this,state_count,_states);
    }
    
    //부모의 추상 메소드를 구현, Entity_Manager의 Update에서 반복함
    public void Update()
    {
        //상태 매니저의 Execute실행
        _stateManager.Execute();
    }

    //상태 추가 메소드
    public void AddState(HeadStates hs)
    {
        State<Head> newState = _states[(int)hs];
        _stateManager.AddState(newState);
    }
    
    //상태 제거 메소드
    public void RemoveState(HeadStates hs)
    {
        State<Head> remState = _states[(int)hs];
        _stateManager.RemoveState(remState);
    }
    //상태 있는지 체크
    public bool IsContainState(HeadStates hs)
    {
        return _stateManager._currentState.Contains(_states[(int)hs]);
    }
}