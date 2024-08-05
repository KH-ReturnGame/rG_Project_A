using System;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceive
{
    public void Start()
    {
        Debug.Log(gameObject.name);
        GameManager.inst.AddSendObj(gameObject,0);
    }

    public void SignalChanged(bool signal)
    {
        Debug.Log((signal)?"문열림":"문닫힘");
    }
}