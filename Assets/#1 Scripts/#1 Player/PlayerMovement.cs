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
    
    //화살 머리 합체 관련
    public bool isConnectHead = false;
    
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
        if ((_nowRigidbody==_bodyRigidbody && _body.GetComponent<Body>().IsContainState(BodyStates.IsGround))||
            (_nowRigidbody==_headRigidbody && _head.GetComponent<Head>().IsContainState(HeadStates.IsGround)))
        {
            _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, 0);
            _nowRigidbody.velocity = new Vector2(_nowRigidbody.velocity.x, jumpForce);
        }
    }
    
    //머리, 몸 전환 입력받기
    public void OnChangePlayer(InputAction.CallbackContext context)
    {
        if (isConnectHead)
        {
            return;
        }

        if (context.started)
        {
            ChangeControl(context.control.name);
        }
    }
    
    //머리 몸 화살 전환 함수
    public void ChangeControl(string controlmode)
    {
        switch (controlmode)
        {
            case "q":
            {
                if(controlMode == "Head") return;
                controlMode = "Head";
                _nowRigidbody = _headRigidbody;
                GameManager.Instance.ChangeCameraTarget(_head);
                _arrow.GetComponent<ArrowController>().ActivateArrow(false);
                _arrow.GetComponent<PolygonCollider2D>().isTrigger = false;
                return;
            }
            case "leftShift":
            {
                if(controlMode == "Arrow") return;
                controlMode = "Arrow";
                _arrow.GetComponent<ArrowController>().ActivateArrow(true);
                return;
            }
            case "e":
            {
                if(controlMode == "Body") return;
                controlMode = "Body";
                _nowRigidbody = _bodyRigidbody;
                GameManager.Instance.ChangeCameraTarget(_body);
                _arrow.GetComponent<ArrowController>().ActivateArrow(false);
                _arrow.GetComponent<PolygonCollider2D>().isTrigger = false;
                return;
            }
        }
    }
    
    //현재 머리인지 몸인지 반환
    public string WhatControlPlayer()
    {
        if (_nowRigidbody == _bodyRigidbody)
        {
            return "Body";
        }

        return "Head";
    }
    
    
    
}