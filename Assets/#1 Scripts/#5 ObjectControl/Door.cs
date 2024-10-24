using System;
using UnityEngine;

public class Door : MonoBehaviour, ISignalReceive
{
    //신호 종류, 문 종류, 회전 종류, 회전 각도
    [HideInInspector] public int SignalType;
    [HideInInspector] public string DoorType;
    [HideInInspector] public int RotateType;
    public float angle;
    
    //실제 문 로직 관련
    private float originAngle;
    private Vector2 startpos;
    private Transform rotateTransform;
    
    //문 자체 신호 관련
    private bool Signal = false;
    
    public void Start()
    {
        //게임 매니저를 거쳐서 시그널 매니저에게 본인을 신호 관리 오브젝트에 추가해달라고 본인의 신호 타입과 같이 보내주기
        GameManager.Instance.AddSendObj(gameObject,SignalType);
        
        //회전 타입에 따른 회전 물체 결정
        if (RotateType == 0)
        {
            rotateTransform = transform.GetChild(0).transform;
        }
        else
        {
            rotateTransform = transform;
        }

        //시작 위치
        startpos = new Vector2(transform.position.x, transform.position.y);
    }

    //ISignalReceive 인터페이스에 있는 이벤트 함수 신호가 변경되면 호출됨
    public void SignalChanged(bool signal)
    {
        Signal = signal;
    }
    
    private void Update()
    {
        if (DoorType == "UpDown")
        {
            if (Signal)
            {
                //위 아래로 작동하는 문이고 신호가 true 이면 올리기
                transform.position =
                    Vector3.Lerp(transform.position, new Vector3(startpos.x,startpos.y+4f, 0), 10f*Time.unscaledDeltaTime);
            }
            else
            {
                //위 아래로 작동하는 문이고 신호가 false 이면 내리기
                transform.position =
                    Vector3.Lerp(transform.position, new Vector3(startpos.x, startpos.y, 0), 10f*Time.unscaledDeltaTime);
            }
        }
        else
        {
            if (Signal)
            {
                //회전하는 문이고 신호가 true 이면 회전 ㄱㄱ
                originAngle = rotateTransform.rotation.z;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, targetRotation, 500 * Time.unscaledDeltaTime);
            }
            else
            {
                //회전하는 문이고 신호가 false 이면 원상복구
                Quaternion targetRotation = Quaternion.AngleAxis(originAngle, Vector3.forward);
                rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, targetRotation, 500 * Time.unscaledDeltaTime);
            }
        }
    }
}