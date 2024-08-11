using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //플레이어 기본 움직임
    private float _movementInputDirection;
    public float _movementSpeed;
    public float _jumpForce;

    //몸 관련
    public GameObject Body;
    private Rigidbody2D BodyRigidbody;
    
    //머리 관련
    public GameObject Head;
    private Rigidbody2D HeadRigidbody;

    //현재 조작할 오브젝트
    private Rigidbody2D NowRigidbody;
    
    //화살 머리 합체 관련
    public bool isConnectHead = false;
    
    //기본 초기화
    private void Start()
    {
        BodyRigidbody = Body.GetComponent<Rigidbody2D>();
        HeadRigidbody = Head.GetComponent<Rigidbody2D>();
        NowRigidbody = BodyRigidbody;
    }

    //움직임 무한 적용~~
    private void Update()
    {
        if(NowRigidbody==HeadRigidbody && isConnectHead){return;}
        
        //기본 좌우 움직임
        NowRigidbody.velocity = new Vector2(_movementInputDirection * _movementSpeed, NowRigidbody.velocity.y);
    }
    
    //좌우 입력받기
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInputDirection = context.ReadValue<float>();
    }
    
    //점프 입력받기
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        if ((NowRigidbody==BodyRigidbody && Body.GetComponent<Body>().IsContainState(BodyStates.IsGround))||
            (NowRigidbody==HeadRigidbody && Head.GetComponent<Head>().IsContainState(HeadStates.IsGround)))
        {
            NowRigidbody.velocity = new Vector2(NowRigidbody.velocity.x, 0);
            NowRigidbody.velocity = new Vector2(NowRigidbody.velocity.x, _jumpForce);
        }
    }
    
    //머리, 몸 전환 입력받기
    public void OnChangePlayer(InputAction.CallbackContext context)
    {
        if (isConnectHead)
        {
            return;
        }
        ChangeControl();
    }
    
    //머리 몸 전환 함수
    public void ChangeControl()
    {
        if (NowRigidbody == BodyRigidbody)
        {
            NowRigidbody = HeadRigidbody;
            GameManager.Instance.ChangeCameraTarget(Head);
        }
        else
        {
            NowRigidbody = BodyRigidbody;
            GameManager.Instance.ChangeCameraTarget(Body);
        }
    }
    
    //현재 머리인지 몸인지 반환
    public string WhatControlPlayer()
    {
        if (NowRigidbody == BodyRigidbody)
        {
            return "Body";
        }

        return "Head";
    }
    
    
    
}