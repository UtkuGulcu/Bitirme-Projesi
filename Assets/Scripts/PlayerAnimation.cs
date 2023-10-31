using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerController playerController;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private static readonly int IsWalkingID = Animator.StringToHash("isWalking");
    private static readonly int ShootID = Animator.StringToHash("shoot");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerController.IsMoving())
        {
            SetupWalkingAnimation();
        }
        else
        {
            SetupIdleAnimation();
        }
    }

    public void OnDirectionChanged(object sender, object data)
    {
        GameEventArgs.OnDirectionChanged args = data as GameEventArgs.OnDirectionChanged;

        spriteRenderer.flipX = args.newDirection == InputManager.Direction.Left;
    }

    public void OnShootButtonDown()
    {
        animator.SetTrigger(ShootID);
    }

    private void SetupIdleAnimation()
    {
        animator.SetBool(IsWalkingID, false);
    }

    private void SetupWalkingAnimation()
    {
        animator.SetBool(IsWalkingID, true);
    }
}
