using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _movementInputDirection;
    private float _movementSpeed = 8.00f;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
        playerRigidbody.velocity = new Vector2(_movementInputDirection * _movementSpeed, playerRigidbody.velocity.y);
    }


    //입력받기
    public void OnMove(InputValue value)
    {
        _movementInputDirection = value.Get<float>();
    }
    
}