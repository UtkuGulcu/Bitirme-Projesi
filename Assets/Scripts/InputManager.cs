using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left
    }
    
    public static InputManager Instance { get; private set; }
    
    [Header("Events")]
    [SerializeField] private GameEventSO OnDirectionChanged;
    [SerializeField] private GameEventSO OnShootButtonDown;
    
    private float horizontalInput;
    private Direction direction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are multiple Input Managers!!");
            Destroy(this);
        }
    }

    private void Update()
    {
        ReadInput();
        HandleDirectionChange();
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnShootButtonDown.Raise();
        }
    }

    private void HandleDirectionChange()
    {
        if (horizontalInput == 0)
        {
            return;
        }
        
        Direction newDirection = horizontalInput > 0 ? Direction.Right : Direction.Left;

        if (direction != newDirection)
        {
            OnDirectionChanged.Raise(this, new GameEventArgs.OnDirectionChanged
            {
                newDirection = newDirection
            });
        }

        direction = newDirection;
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
}
