using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationSystem : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left
    }

    public static AnimationSystem Instance { get; private set; }

    [Header("Upper Body and Legs References")]
    [SerializeField] private Transform upperBody;
    [SerializeField] private Transform legs;

    [Header("Events")] 
    [SerializeField] private GameEventSO OnDirectionChanged;
    
    private Direction direction;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are multiple Animation Systems!!");
            Destroy(this);
        }
        
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetupIdleAnimationLeft();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetupIdleAnimationRight();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetupWalkingAnimationRight();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ShootAnimation();
        }
    }

    private void SetupIdleAnimationRight()
    {
        animator.SetBool("isWalking", false);
        direction = Direction.Right;
        InvokeOnDirectionChangedEvent(direction);
        spriteRenderer.flipX = false;
    }
    
    private void SetupIdleAnimationLeft()
    {
        animator.SetBool("isWalking", false);
        direction = Direction.Left;
        InvokeOnDirectionChangedEvent(direction);
        spriteRenderer.flipX = true;
    }
    
    private void SetupWalkingAnimationRight()
    {
        animator.SetBool("isWalking", true);
    }
    
    private void ShootAnimation()
    {
        animator.SetTrigger("shoot");
    }

    public Direction GetDirection()
    {
        return direction;
    }

    private void InvokeOnDirectionChangedEvent(Direction direction)
    {
        OnDirectionChanged.Raise(this, new GameEventArgs.OnDirectionChanged
        {
            newDirection = direction
        });
    }
}
