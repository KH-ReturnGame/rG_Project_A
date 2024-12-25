using System;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, ISignalReceive
{
    //신호 종류, 문 종류, 회전 종류, 회전 각도
    [HideInInspector] public int SignalType;
    [HideInInspector] public string DoorType;
    [HideInInspector] public int RotateType;
    [HideInInspector] public int xyType;
    
    public int push = 1;
    
    public float angle;
    public int updown = 1;
    
    //실제 문 로직 관련
    private float originAngle;
    private Vector2 startpos;
    private Transform rotateTransform;
    
    //문 자체 신호 관련
    public bool Signal = false;
    
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
        
        // 첫 번째 값은 일반 tweens, 두 번째 값은 시퀀스 tweens 용량입니다.
        DOTween.SetTweensCapacity(2000, 100); // 2000개의 일반 tweens, 100개의 시퀀스를 설정
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
            if (xyType == 0)
            {
                if (Signal)
                {
                    transform.position =
                        Vector3.Lerp(transform.position, new Vector3(startpos.x,startpos.y+(5f*updown), 0), 20f*Time.unscaledDeltaTime);
                }
                else
                {
                    transform.position =
                        Vector3.Lerp(transform.position, new Vector3(startpos.x, startpos.y, 0), 20f*Time.unscaledDeltaTime);
                }
            }
            else
            {
                if (Signal)
                {
                    transform.position =
                        Vector3.Lerp(transform.position, new Vector3(startpos.x+(5f*updown),startpos.y, 0), 20f*Time.unscaledDeltaTime);
                }
                else
                {
                    transform.position =
                        Vector3.Lerp(transform.position, new Vector3(startpos.x, startpos.y, 0), 20f*Time.unscaledDeltaTime);
                }
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