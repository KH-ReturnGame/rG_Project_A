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
        Debug.Log("이거는 ??????");
        Signal[index] = signal;
        
        var items = SendObj.Where(pair => pair.Value == index);
        foreach (var item in items)
        {
            item.Key.GetComponent<ISignalReceive>().SignalChanged(signal);
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