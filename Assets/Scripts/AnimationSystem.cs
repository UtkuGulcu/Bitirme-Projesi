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
    
    [Header("Animators")] 
    [SerializeField] private Animator upperBodyAnimator;
    [SerializeField] private Animator legsAnimator;

    [Header("Upper Body and Legs References")]
    [SerializeField] private Transform upperBody;
    [SerializeField] private Transform legs;

    [Header("Events")] 
    [SerializeField] private GameEventSO OnDirectionChanged;

    private SpriteRenderer upperBodySpriteRenderer;
    private SpriteRenderer legsSpriteRenderer;
    private Direction direction;

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
        
        upperBodySpriteRenderer = upperBody.GetComponent<SpriteRenderer>();
        legsSpriteRenderer = legs.GetComponent<SpriteRenderer>();
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
            SetupIdleShootAnimation();
        }
    }

    private void SetupIdleAnimationRight()
    {
        direction = Direction.Right;
        InvokeOnDirectionChangedEvent(direction);
        upperBodyAnimator.SetBool("isWalking",false);
        legsAnimator.SetBool("isWalking",false);
        //upperBody.localPosition = idleAnimationUpperBodyOffsetRight;
        legsSpriteRenderer.flipX = false;
        upperBodySpriteRenderer.flipX = false;
    }
    
    private void SetupIdleAnimationLeft()
    {
        direction = Direction.Left;
        InvokeOnDirectionChangedEvent(direction);
        upperBodyAnimator.SetBool("isWalking",false);
        legsAnimator.SetBool("isWalking",false);
        //upperBody.localPosition = idleAnimationUpperBodyOffsetLeft;
        legsSpriteRenderer.flipX = true;
        upperBodySpriteRenderer.flipX = true;
    }
    
    private void SetupWalkingAnimationRight()
    {
        upperBodyAnimator.SetBool("isWalking", true);
        legsAnimator.SetBool("isWalking", true);
        //upperBody.localPosition = walkAnimationUpperBodyOffsetRight;
    }
    
    private void SetupIdleShootAnimation()
    {
        upperBodyAnimator.SetTrigger("shoot");
        legsAnimator.SetTrigger("shoot");
        //upperBody.localPosition = idleShootAnimationUpperBodyOffsetRight;
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
