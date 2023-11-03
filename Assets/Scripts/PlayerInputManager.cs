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
        // if (Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Debug.LogError("There are multiple Input Managers!!");
        //     Destroy(this);
        // }

        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnShootButtonDown(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }
        
        player.TryToShoot();
        Debug.Log($"Player {playerInput.playerIndex} is shooting");
    }

    private void Update()
    {
        ReadMovementInput();
    }

    private void ReadMovementInput()
    {
        Vector2 inputVector = playerInput.actions["Movement"].ReadValue<Vector2>();

        horizontalInput = inputVector.x;
    }
    
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
}
