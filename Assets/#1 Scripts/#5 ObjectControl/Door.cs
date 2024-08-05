using System;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceive
{
    [HideInInspector] public int SignalType;
    
    public void Start()
    {
        Debug.Log(gameObject.name);
        GameManager.inst.AddSendObj(gameObject,SignalType);
    }

    public void SignalChanged(bool signal)
    {
        Debug.Log((signal)?"문열림":"문닫힘");
    }
}