using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerAnimation playerAnimation;
    
    [Header("Movement Related Values")]
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velocityPower;
    [SerializeField] private float frictionAmount;

    [Header("Gravity Related Values")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;

    [Header("Layers To Drop Down")] 
    [SerializeField] private LayerMask layersToDropDown;

    private Rigidbody2D rb;
    private PlayerInputManager playerInputManager;
    private BoxCollider2D colliderPlayer;
    private readonly Vector3 leftDirectionAngles = new Vector3(0, 180, 0);
    private float previousGravityY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInputManager = GetComponent<PlayerInputManager>();
        colliderPlayer = GetComponent<BoxCollider2D>();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        HandleFriction();
        HandleGravity();
        HandleDirection();
        HandleFallingDetection();
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

    private void HandleDirection()
    {
        float horizontalInput = playerInputManager.GetHorizontalInput();
        
        if (horizontalInput == 0)
        {
            return;
        }
        
        transform.eulerAngles = horizontalInput > 0 ? Vector3.zero: leftDirectionAngles;
    }

    private void HandleGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -20));
        }
    }

    private void HandleFallingDetection()
    {
        switch (previousGravityY)
        {
            case >= 0 when rb.velocity.y < 0:
                playerAnimation.PlayFallingAnimation();
                break;
            case < 0 when rb.velocity.y == 0:
                playerAnimation.StopFallingAnimation();
                break;
        }

        previousGravityY = rb.velocity.y;
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void DropDown()
    {
        Vector2 startPosition = colliderPlayer.bounds.center;
        float range = colliderPlayer.bounds.extents.y + 0.2f;
        
        RaycastHit2D raycastHit = Physics2D.Raycast(startPosition, Vector2.down, range, layersToDropDown);

        if (raycastHit.collider == null)
        {
            return;
        }

        StartCoroutine(IgnoreCollision(raycastHit.collider));
    }
    
    public bool IsMoving()
    {
        return Mathf.Abs(playerInputManager.GetHorizontalInput()) > 0.01f;
    }

    private IEnumerator IgnoreCollision(Collider2D colliderToIgnore)
    {
        Physics2D.IgnoreCollision(colliderPlayer, colliderToIgnore);
        yield return Helpers.GetWait(0.5f);
        Physics2D.IgnoreCollision(colliderPlayer, colliderToIgnore, false);
    }
}
