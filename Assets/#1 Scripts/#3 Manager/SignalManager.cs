using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SignalManager : MonoBehaviour
{
    // 실제 신호 값들
    private bool[] Signal = new bool[20];

    // 어떤 물체는 어떤 신호가 변경되었을 때 반응할지
    private Dictionary<GameObject, int> SendObj = new Dictionary<GameObject, int>();

    // 어떤 물체가 어떤 신호를 주는지
    private Dictionary<GameObject, int> ChangeObj = new Dictionary<GameObject, int>();

    // Signal 초기화
    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Signal[i] = false;
        }
    }

    // 특정 신호 체크
    public bool CheckSignal(int index)
    {
        return Signal[index];
    }

    // 특정 신호 변경 (하나의 신호를 OR 연산을 통해 계산하도록 수정)
    public void ChangeSignal(int index, bool signal)
    {
        // 게임 중간에 제거된 물체 저장할 리스트
        var items_change_ToRemove = new List<GameObject>();

        bool finalSignal = false;
        
        // 변경한 신호를 변경하는 물체 리스트를 딕셔너리에서 찾기
        var items_change = ChangeObj.Where(pair => pair.Value == index);
        foreach (var item in items_change)
        {
            if (item.Key == null || !item.Key.activeInHierarchy)
            {
                items_change_ToRemove.Add(item.Key);
                continue;
            }

            if (item.Key.tag == "Button")
            {
                finalSignal = finalSignal || item.Key.GetComponent<FootHoldButton>().Signal;
            }
            else
            {
                finalSignal = finalSignal || item.Key.GetComponent<ArrowCreate>().Signal;
            }
            
        }
        Signal[index] = finalSignal;
        
        
        // 신호가 true로 변경되면 모든 관련 오브젝트들의 상태를 OR 연산을 통해 결합
        //Signal[index] = signal || SendObj.Where(pair => pair.Value == index)
        //    .Any(pair => pair.Key != null && pair.Key.activeInHierarchy && pair.Key.GetComponent<FootHoldButton>()?.signal == true);

        // 여기 아래는 변경한 신호에 영향을 받는 오브젝트들의 이벤트 함수를 호출하는 부분

        // 게임 중간에 제거된 물체 저장할 리스트
        var itemsToRemove = new List<GameObject>();
        
        // 변경한 신호와 상호작용하는 물체 리스트를 딕셔너리에서 찾기
        var items = SendObj.Where(pair => pair.Value == index);
        foreach (var item in items)
        {
            // 만약 그 물체가 없으면? -> 제거 리스트에 추가
            if (item.Key == null || !item.Key.activeInHierarchy)
            {
                itemsToRemove.Add(item.Key);
                continue;
            }

            // 신호와 상호작용하는 물체 중 하나에서 ISignalReceive라는 인터페이스를 가져오기
            var signalReceiver = item.Key.GetComponent<ISignalReceive>();
            if (signalReceiver != null)
            {
                // 아무런 문제가 없으면 신호가 바뀌었다고 알려주기
                signalReceiver.SignalChanged(Signal[index]);
            }
            else
            {
                itemsToRemove.Add(item.Key);
            }
        }

        // 파괴되었거나 유효하지 않은 오브젝트 제거
        foreach (var obj in itemsToRemove)
        {
            SendObj.Remove(obj);
        }
        // 파괴되었거나 유효하지 않은 오브젝트 제거
        foreach (var obj in items_change_ToRemove)
        {
            SendObj.Remove(obj);
        }
    }

    // 어떤 물체가 어떤 신호와 상호작용하는지 딕셔너리 추가 함수
    public void AddSendObj(GameObject obj, int index)
    {
        SendObj.Add(obj, index);
    }

    // 딕셔너리 제거 함수
    public void RemSendObj(GameObject obj)
    {
        SendObj.Remove(obj);
    }
    
    // 어떤 물체가 어떤 신호를 주는지 딕셔너리 추가 함수
    public void AddChangeObj(GameObject obj, int index)
    {
        //Debug.Log(obj.name + "을 "+index+"번 신호로 관리 시작했습니다.");
        ChangeObj.Add(obj, index);
    }
    
    //딕셔너리 제거 함수
    public void RemChangeObj(GameObject obj)
    {
        ChangeObj.Remove(obj);
    }
}

//이벤트 함수를 가지고 있는 인터페이스
interface ISignalReceive
{
    void SignalChanged(bool signal);
}