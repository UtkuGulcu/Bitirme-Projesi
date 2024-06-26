using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerController playerController;

    private static readonly int isWalkingID = Animator.StringToHash("isWalking");
    private static readonly int shootID = Animator.StringToHash("shoot");
    private static readonly int jumpID = Animator.StringToHash("jump");
    private static readonly int fallingStartedID = Animator.StringToHash("fallingStarted");
    private static readonly int fallingFinishedID = Animator.StringToHash("fallingFinished");
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerController.IsMoving())
        {
            PlayWalkingAnimation();
        }
        else
        {
            PlayIdleAnimation();
        }
    }
  
    public void PlayJumpAnimation()
    {
        animator.SetTrigger(jumpID);
    }

    public void PlayFallingAnimation()
    {
        animator.SetTrigger(fallingStartedID);
    }

    public void StopFallingAnimation()
    {
        animator.SetTrigger(fallingFinishedID);
    }
  
    public void PlayShootAnimation()
    {
        animator.SetTrigger(shootID);
    }

    private void PlayIdleAnimation()
    {
        animator.SetBool(isWalkingID, false);
    }

    private void PlayWalkingAnimation()
    {
        animator.SetBool(isWalkingID, true);
    }
}
