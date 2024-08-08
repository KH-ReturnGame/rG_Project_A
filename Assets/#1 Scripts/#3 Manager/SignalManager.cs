using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SignalManager : MonoBehaviour
{
    //실제 신호 값들
    private bool[] Signal = new bool[10];
    
    //어떤 물체는 어떤 신호가 변경되었을때 반응할지
    private Dictionary<GameObject, int> SendObj = new Dictionary<GameObject, int>();

    //Signal 초기화
    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            Signal[i] = false;
        }
    }

    //특정 신호 체크
    public bool CheckSignal(int index)
    {
        return Signal[index];
    }

    //특정 신호 변경
    public void ChangeSignal(int index, bool signal)
    {
        //실제 변경 부분
        Signal[index] = signal;
        
        //여기 아래는 변경한 신호에 영향을 받는 오브젝트들의 이벤트 함수를 호출하는 부분
        //그냥 신호 영향받는 오브젝트들에서 CheckSignal 함수 Update에서 호출하면서 변경 확인해도 되는데 Interface 사용해서 좀 더 최적화 해봤음
        
        //게임 중간에 제거된 물체 저장할 리스트
        var itemsToRemove = new List<GameObject>();
        
        //변경한 신호와 상호작용하는 물체 리스트를 딕셔너리에서 찾기
        var items = SendObj.Where(pair => pair.Value == index);
        foreach (var item in items)
        {
            //만약 그 물체가 없으면? -> 제거 리스트에 추가
            if (item.Key == null || !item.Key.activeInHierarchy)
            {
                itemsToRemove.Add(item.Key);
                continue;
            }
            
            //신호와 상호작용하는 물체중 하나에서 ISignalReceive라는 인터페이스를 가져오기
            var signalReceiver = item.Key.GetComponent<ISignalReceive>();
            if (signalReceiver != null)
            {
                //아무런 문제가 없으면 신호가 바뀌었다고 알려주기
                signalReceiver.SignalChanged(signal);
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
    }
    
    //어떤 물체가 어떤 신호와 상호작용하는지 딕셔너리 추가 함수
    public void AddSendObj(GameObject obj, int index)
    {
        //Debug.Log(obj.name);
        SendObj.Add(obj,index);
    }
    
    //딕셔너리 제거 함수
    public void RemSendObj(GameObject obj)
    {
        SendObj.Remove(obj);
    }
}

//이벤트 함수를 가지고 있는 인터페이스
interface ISignalReceive
{
    void SignalChanged(bool signal);
}