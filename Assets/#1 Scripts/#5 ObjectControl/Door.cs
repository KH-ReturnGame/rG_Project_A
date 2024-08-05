using System;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceive
{
    [HideInInspector] public int SignalType;
    [HideInInspector] public string DoorType;
    [HideInInspector] public int RotateType;
    public float angle;
    private float originAngle;
    private Vector2 startpos;

    private Transform rotateTransform;
    
    private bool Signal = false;
    
    
    
    public void Start()
    {
        Debug.Log(gameObject.name);
        GameManager.inst.AddSendObj(gameObject,SignalType);
        
        if (RotateType == 0)
        {
            rotateTransform = transform.GetChild(0).transform;
        }
        else
        {
            rotateTransform = transform;
        }

        startpos = new Vector2(transform.position.x, transform.position.y);
        //Debug.Log(startpos);
    }

    public void SignalChanged(bool signal)
    {
        //Debug.Log((signal)?"문열림":"문닫힘");
        Signal = signal;
    }

    private void Update()
    {
        if (DoorType == "UpDown")
        {
            if (Signal)
            {
                transform.position =
                    Vector3.Lerp(transform.position, new Vector3(startpos.x,startpos.y+4f, 0), 10f*Time.deltaTime);
            }
            else
            {
                transform.position =
                    Vector3.Lerp(transform.position, new Vector3(startpos.x, startpos.y, 0), 10f*Time.deltaTime);
            }
        }
        else
        {
            if (Signal)
            {
                originAngle = rotateTransform.rotation.z;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, targetRotation, 500 * Time.deltaTime);
            }
            else
            {
                Quaternion targetRotation = Quaternion.AngleAxis(originAngle, Vector3.forward);
                rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, targetRotation, 500 * Time.deltaTime);
            }
        }
    }
}