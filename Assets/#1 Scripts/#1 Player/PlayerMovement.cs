using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _movementInputDirection;
    private float _movementSpeed = 8.00f;
    private float _jumpForce = 10f;

    public GameObject Body;
    private Rigidbody2D BodyRigidbody;
    
    public GameObject Head;
    private Rigidbody2D HeadRigidbody;

    private Rigidbody2D NowRigidbody;
    
    

    private void Start()
    {
        BodyRigidbody = Body.GetComponent<Rigidbody2D>();
        HeadRigidbody = Head.GetComponent<Rigidbody2D>();
        NowRigidbody = BodyRigidbody;
    }

    private void Update()
    {
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
        
        if (NowRigidbody == BodyRigidbody)
        {
            NowRigidbody = HeadRigidbody;
            GameManager.inst.ChangeCameraTarget(Head);
        }
        else
        {
            NowRigidbody = BodyRigidbody;
            GameManager.inst.ChangeCameraTarget(Body);
        }
    }
    
    
    
}