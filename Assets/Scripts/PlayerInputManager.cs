using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private float horizontalInput;
    private Player player;
    private PlayerInput playerInput;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        ReadMovementInput();
    }
    
    public void OnShootButtonDown(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        
        player.TryToShoot();
    }

    public void OnJumpButtonDown(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        
        player.TryToJump();
    }

    public void OnDropDownButtonDown(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        
        player.TryToDropDown();
    }

    private void ReadMovementInput()
    {
        horizontalInput = playerInput.actions["Movement"].ReadValue<float>();

        switch (horizontalInput)
        {
            case > 0:
                horizontalInput = 1;
                break;
            case < 0:
                horizontalInput = -1;
                break;
        }
    }
    
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
}
