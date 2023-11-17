using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private bool isGrounded;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isGrounded = true;
        player.RefreshDoubleJump();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
