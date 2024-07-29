using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _movementInputDirection;
    private float _movementSpeed = 8.00f;

    public GameObject Body;
    private Rigidbody2D BodyRigidbody;
    
    public GameObject Head;
    private Rigidbody2D HeadRigidbody;

    private void Start()
    {
        BodyRigidbody = Body.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
        BodyRigidbody.velocity = new Vector2(_movementInputDirection * _movementSpeed, BodyRigidbody.velocity.y);
    }


    //입력받기
    public void OnMove(InputValue value)
    {
        _movementInputDirection = value.Get<float>();
    }
    
}