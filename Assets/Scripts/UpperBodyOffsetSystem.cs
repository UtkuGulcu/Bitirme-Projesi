using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperBodyOffsetSystem : MonoBehaviour
{
    private enum AnimationState
    {
        Idle,
        IdleShoot,
        Walking,
        ShooingWhileWalking
    }
    
    [Header("Offsets")]
    [SerializeField] private Vector3 walkAnimationOffsetRight;
    [SerializeField] private Vector3 walkAnimationOffsetLeft;
    [SerializeField] private Vector3 idleAnimationOffsetRight;
    [SerializeField] private Vector3 idleAnimationOffsetLeft;
    [SerializeField] private Vector3 idleShootAnimationOffsetRight;
    [SerializeField] private Vector3 idleShootAnimationOffsetLeft;
    [SerializeField] private Vector3 shootWhileWalkingAnimationOffsetRight;
    [SerializeField] private Vector3 shootWhileWalkingAnimationOffsetLeft;

    private AnimationState animationState;

    // Animation Event
    private void OnIdleAnimationStarted()
    {
        ChangeAnimationState(AnimationState.Idle);
        transform.localPosition = AnimationSystem.Instance.GetDirection() == AnimationSystem.Direction.Right ? idleAnimationOffsetRight : idleAnimationOffsetLeft;
    }
    
    // Animation Event
    private void OnIdleShootAnimationStarted()
    {
        ChangeAnimationState(AnimationState.IdleShoot);
        transform.localPosition = AnimationSystem.Instance.GetDirection() == AnimationSystem.Direction.Right ? idleShootAnimationOffsetRight : idleShootAnimationOffsetLeft;
    }
    
    // Animation Event
    private void OnWalkingAnimationStarted()
    {
        ChangeAnimationState(AnimationState.Walking);
        transform.localPosition = AnimationSystem.Instance.GetDirection() == AnimationSystem.Direction.Right ? walkAnimationOffsetRight : walkAnimationOffsetLeft;
    }
    
    // Animation Event
    private void OnShootingWhileWalkingAnimationStarted()
    { 
        ChangeAnimationState(AnimationState.ShooingWhileWalking);
        transform.localPosition = AnimationSystem.Instance.GetDirection() == AnimationSystem.Direction.Right ? shootWhileWalkingAnimationOffsetRight : shootWhileWalkingAnimationOffsetLeft;
    }

    public void OnDirectionChanged(object sender, object data)
    {
        GameEventArgs.OnDirectionChanged args = data as GameEventArgs.OnDirectionChanged;
        AnimationSystem.Direction direction = args.newDirection;

        transform.localPosition = animationState switch
        {
            AnimationState.Idle => direction == AnimationSystem.Direction.Right
                ? idleAnimationOffsetRight
                : idleAnimationOffsetLeft,
            
            AnimationState.IdleShoot => direction == AnimationSystem.Direction.Right
                ? idleShootAnimationOffsetRight
                : idleShootAnimationOffsetLeft,
            
            AnimationState.Walking => direction == AnimationSystem.Direction.Right
                ? walkAnimationOffsetRight
                : walkAnimationOffsetLeft,
            
            AnimationState.ShooingWhileWalking => direction == AnimationSystem.Direction.Right
                ? shootWhileWalkingAnimationOffsetRight
                : shootWhileWalkingAnimationOffsetLeft,
            
            _ => Vector3.zero
        };
    }

    private void ChangeAnimationState(AnimationState newAnimationState)
    {
        animationState = newAnimationState;
    }
}
