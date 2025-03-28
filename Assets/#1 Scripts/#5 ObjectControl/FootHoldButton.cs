using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
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
    public bool oneUse = false;
    private int useCount = 0;
    public bool onOnly = false;
    public bool canClickArrow = true;
    
    //버튼 자체 신호
    public bool Signal = false;

    //버튼 눌림 관련해서 누가 눌렀는지
    public List<Collider2D> downObj;

    private void Start()
    {
        GameManager.Instance.AddChangeObj(gameObject,SignalType);
    }

    //버튼 눌림을 위한 충돌 시작할때 이벤트 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        //감지된 오브젝트 눌림 리스트에 추가 감지하지 않을 놈들이면 그냥 return;
        if ((other.CompareTag("Head") || other.CompareTag("Body")))
        {
            downObj.Add(other);
        }
        else if (other.CompareTag("Arrow") && canClickArrow)
        {
            downObj.Add(other);
        }
        else
        {
            return;
        }
        
        //Debug.Log(oneUse+""+useCount);
        if ((oneUse && useCount >= 1) /*|| (other.CompareTag("Arrow") && GameObject.FindWithTag("Player").GetComponent<Player>().IsContainState(PlayerStats.CanControlArrow))*/)
        {
            return;
        }
        
        //만약 단 한개의 오브젝트만 감지되었고 아직 안눌렸다면? ++ 토글 버튼이 아니라면
        if (downObj.Count == 1 && !Signal && !isToggle)
        {
            Signal = true;
            GameManager.Instance.ChangeSignal(SignalType, Signal);
            useCount += onOnly ? 1 : 0;
            moveButton("enter");
        }

        //만약 단 한개의 오브젝트만 감지되었고 토글 버튼이라면?
        if (downObj.Count == 1 && isToggle)
        {
            Signal = !Signal;
            GameManager.Instance.ChangeSignal(SignalType,Signal);
            if (Signal)
            {
                useCount += onOnly ? 1 : 0;
                moveButton("enter");
            }
            else
            {
                useCount += onOnly ? 0 : 1;
                moveButton("exit");
            }
        }
    }

    //버튼 눌림을 위한 충돌 해제 이벤트 함수
    private void OnTriggerExit2D(Collider2D other)
    {
        //감지된 오브젝트 눌림 리스트에 제거 감지하지 않을 놈들이면 그냥 return;
        if (other.CompareTag("Head") || other.CompareTag("Body"))
        {
            downObj.Remove(other);
        }
        else if ((other.CompareTag("Arrow") && canClickArrow))
        {
            downObj.Remove(other);
        }
        else
        {
            return;
        }
        
        if ((oneUse && useCount >= 1))
        {
            return;
        }
        
        //만약 버튼위에 아무 오브젝트도 없고 눌린 상태라면? ++ 토글 버튼이 아니라면
        if (downObj.Count == 0 && Signal && !isToggle)
        {
            Signal = false;
            GameManager.Instance.ChangeSignal(SignalType,Signal);
            useCount += onOnly ? 0 : 1;
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
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y - 0.1f*flip*f,
                transform.GetChild(0).transform.position.z);
        }
        
        //눌리는 방향이 x방향인지
        else
        {
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x - 0.1f*flip*f, transform.GetChild(0).transform.position.y,
                transform.GetChild(0).transform.position.z);
        }
    }

    //신호에 따른 생상 변화
    public void Update()
    {
        //919191,96FF7F
        Color color;
        if (Signal)
        {
            ColorUtility.TryParseHtmlString("#96FF7F",out color);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#919191",out color);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
    }
}
