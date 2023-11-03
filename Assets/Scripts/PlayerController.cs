using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Controller Values")]
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velocityPower;
    [SerializeField] private float frictionAmount;

    private Rigidbody2D rb;
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        HandleFriction();
    }

    private void HandleMovement()
    {
        float targetSpeed = playerInputManager.GetHorizontalInput() * maxMovementSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) * Mathf.Sign(speedDifference);
        
        rb.AddForce(movement * Vector2.right);
    }
    
    private void HandleFriction()
    {
        if (Mathf.Abs(playerInputManager.GetHorizontalInput()) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), frictionAmount);
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    
    public bool IsMoving()
    {
        return Mathf.Abs(playerInputManager.GetHorizontalInput()) > 0.01f;
    }
}
