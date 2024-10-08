using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class FootHoldButton : MonoBehaviour
{
    //버튼 종류 관련 변수
    public bool isToggle = true;
    [HideInInspector]
    public string downDirection = "X";
    [HideInInspector]
    public int flip = 1;
    public int SignalType = 0;
    
    //버튼 자체 신호
    public bool signal = false;

    //버튼 눌림 관련해서 누가 눌렀는지
    public List<Collider2D> downObj;

    //버튼 눌림을 위한 충돌 시작할때 이벤트 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        //감지된 오브젝트 눌림 리스트에 추가 감지하지 않을 놈들이면 그냥 return;
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Add(other);
        }
        else
        {
            return;
        }
        
        //만약 단 한개의 오브젝트만 감지되었고 아직 안눌렸다면? ++ 토글 버튼이 아니라면
        if (downObj.Count == 1 && !signal && !isToggle)
        {
            signal = true;
            GameManager.Instance.ChangeSignal(SignalType, signal);
            moveButton("enter");
        }

        //만약 단 한개의 오브젝트만 감지되었고 토글 버튼이라면?
        if (downObj.Count == 1 && isToggle)
        {
            signal = !signal;
            GameManager.Instance.ChangeSignal(SignalType,signal);
            if (signal)
            {
                moveButton("enter");
            }
            else
            {
                moveButton("exit");
            }
        }
    }

    //버튼 눌림을 위한 충돌 해제 이벤트 함수
    private void OnTriggerExit2D(Collider2D other)
    {
        //감지된 오브젝트 눌림 리스트에 제거 감지하지 않을 놈들이면 그냥 return;
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Arrow"))
        {
            downObj.Remove(other);
        }
        else
        {
            return;
        }
        
        //만약 버튼위에 아무 오브젝트도 없고 눌린 상태라면? ++ 토글 버튼이 아니라면
        if (downObj.Count == 0 && signal && !isToggle)
        {
            signal = false;
            GameManager.Instance.ChangeSignal(SignalType,signal);
            moveButton("exit");
        }
    }

    //버튼 눌림 모션 구현
    private void moveButton(string str)
    {
        //눌림 상태 진입 이라면 반전 X, -> else -1 ( 삼항 연산자 공부하세욧 )
        int f = str == "enter" ? 1 : -1;
        
        //그리고 눌리는 방향이 y방향인지
        if (downDirection == "Y")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f*flip*f,
                transform.position.z);
        }
        
        //눌리는 방향이 x방향인지
        else
        {
            transform.position = new Vector3(transform.position.x - 0.2f*flip*f, transform.position.y,
                transform.position.z);
        }
    }

    //신호에 따른 생상 변화
    public void Update()
    {
        //919191,96FF7F
        Color color;
        if (signal)
        {
            ColorUtility.TryParseHtmlString("#96FF7F",out color);
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#919191",out color);
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
