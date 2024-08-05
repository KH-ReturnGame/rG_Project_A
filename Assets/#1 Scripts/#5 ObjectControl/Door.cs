using System;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceive
{
    [HideInInspector] public int SignalType;
    [HideInInspector] public string DoorType;
    private bool Signal = false;
    
    public void Start()
    {
        Debug.Log(gameObject.name);
        GameManager.inst.AddSendObj(gameObject,SignalType);
    }

    public void SignalChanged(bool signal)
    {
        Debug.Log((signal)?"문열림":"문닫힘");
        Signal = signal;
    }

    private void Update()
    {
        if (DoorType == "UpDown")
        {
            if (Signal)
            {
                transform.position =
                    Vector3.Slerp(transform.position, new Vector3(0, 4, 0), 10f*Time.deltaTime);
            }
            else
            {
                transform.position =
                    Vector3.Slerp(transform.position, new Vector3(0, 0, 0), 10f*Time.deltaTime);
            }
        }
        else
        {
            
        }
    }
}