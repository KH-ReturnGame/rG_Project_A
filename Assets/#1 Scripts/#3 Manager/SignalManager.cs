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

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            Signal[i] = false;
        }
        //Debug.Log("list size" + Signal.Length);
    }

    public bool CheckSignal(int index)
    {
        return Signal[index];
    }

    public void ChangeSignal(int index, bool signal)
    {
        Signal[index] = signal;
        
        var itemsToRemove = new List<GameObject>();
        var items = SendObj.Where(pair => pair.Value == index);
        foreach (var item in items)
        {
            if (item.Key == null || !item.Key.activeInHierarchy)
            {
                itemsToRemove.Add(item.Key);
                continue;
            }
            
            var signalReceiver = item.Key.GetComponent<ISignalReceive>();
            if (signalReceiver != null)
            {
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
    public void AddSendObj(GameObject obj, int index)
    {
        //Debug.Log(obj.name);
        SendObj.Add(obj,index);
    }
    public void RemSendObj(GameObject obj)
    {
        SendObj.Remove(obj);
    }
}

interface ISignalReceive
{
    void SignalChanged(bool signal);
}