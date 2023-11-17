using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private GroundedCheck groundedCheck;

    private PlayerController playerController;
    private bool canDoubleJump = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void TryToShoot()
    {
        playerAnimation.PlayShootAnimation();
    }

    public void TryToJump()
    {
        if (groundedCheck.IsGrounded())
        {
            playerController.Jump();
            playerAnimation.PlayJumpAnimation();    
        }
        else if (canDoubleJump)
        {
            playerController.Jump();
            playerAnimation.PlayJumpAnimation();
            canDoubleJump = false;
        }
    }

    public void TryToDropDown()
    {
        if (groundedCheck.IsGrounded())
        {
            playerController.DropDown();
        }
    }

    public void RefreshDoubleJump()
    {
        canDoubleJump = true;
    }
}
