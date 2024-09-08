using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    
    //플레이어 기본 움직임
    private float _movementInputDirection;
    public float movementSpeed;
    public float jumpForce;

    //몸 관련
    private GameObject _body;
    private Rigidbody2D _bodyRigidbody;
    
    //머리 관련
    private GameObject _head;
    private Rigidbody2D _headRigidbody;
    
    //화살 관련
    private GameObject _arrow;

    //현재 조작할 오브젝트
    private Rigidbody2D _nowRigidbody;
    public string controlMode;
    
    //기본 초기화
    private void Start()
    {
        _body = player.GetPlayerObj(PlayerObj.Body);
        _bodyRigidbody = _body.GetComponent<Rigidbody2D>();
        _head = player.GetPlayerObj(PlayerObj.Head);
        _headRigidbody = _head.GetComponent<Rigidbody2D>();
        _arrow = player.GetPlayerObj(PlayerObj.Arrow);
        _nowRigidbody = _bodyRigidbody;
        controlMode = "Body";
    }

    //움직임 무한 적용~~
    private void Update()
    {
        if (controlMode == "Arrow") return;
        //기본 좌우 움직임
        _nowRigidbody.velocity = new Vector2(_movementInputDirection * movementSpeed, _nowRigidbody.velocity.y);
    }
    
    //좌우 입력받기
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInputDirection = context.ReadValue<float>();
    }
    
    //점프 입력받기
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started || controlMode == "Arrow")
            return;
        if ((_nowRigidbody==_bodyRigidbody && player.IsContainState(PlayerStats.BodyIsGround))||
            (_nowRigidbody==_headRigidbody && player.IsContainState(PlayerStats.HeadIsGround)))
        {
            _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, 0);
            _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, jumpForce);
        }
    }
    
    //머리, 몸 전환 입력받기
    public void OnChangePlayer(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChangeControl(context.control.name);
        }
    }
    
    //머리 몸 결합 입력받기
    public void OnCombinePlayer(InputAction.CallbackContext context)
    {
        if (context.started )
        {
            if (player.IsContainState(PlayerStats.CanCombine) && !player.IsContainState(PlayerStats.IsCombine))
            {
                //결합
                Debug.Log("결합");
                ChangeControl("q");
                player.RemoveState(PlayerStats.CanCombine);
                player.AddState(PlayerStats.IsCombine);
            }
            else if(player.IsContainState(PlayerStats.IsCombine))
            {
                //결합 해제
                Debug.Log("해제");
                player.AddState(PlayerStats.CanCombine);
                player.RemoveState(PlayerStats.IsCombine);
            }
        }
        
    }
    
    //머리 몸 화살 전환 함수
    public void ChangeControl(string controlmode)
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        
        switch (controlmode)
        {
            case "q":
            {
                if(controlMode == "Head") return;
                controlMode = "Head";
                _arrow.GetComponent<ArrowController>().ActivateArrow(false);
                _arrow.GetComponent<PolygonCollider2D>().isTrigger = false;
                
                _nowRigidbody = _headRigidbody;
                //GameManager.Instance.ChangeCameraTarget(_head);
                return;
            }
            case "leftShift":
            {
                if(controlMode == "Arrow") return;
                controlMode = "Arrow";
                _arrow.GetComponent<ArrowController>().ActivateArrow(true);
                _arrow.GetComponent<PolygonCollider2D>().isTrigger = true;
                return;
            }
            case "e":
            {
                if(controlMode == "Body") return;
                controlMode = "Body";
                _arrow.GetComponent<ArrowController>().ActivateArrow(false);
                _arrow.GetComponent<PolygonCollider2D>().isTrigger = false;
                
                _nowRigidbody = _bodyRigidbody;
                //GameManager.Instance.ChangeCameraTarget(_body);
                return;
            }
        }
    }
}