using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are multiple Lobby Managers!!");
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += InputSystem_OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= InputSystem_OnActionChange;
    }

    private void InputSystem_OnActionChange(object arg1, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed)
        {
            return;
        }

        InputAction inputAction = arg1 as InputAction;
        InputDevice inputDevice = inputAction.activeControl.device;
        List<InputDevice> activeDevices = LobbyPreferences.GetActiveDevices(); 

        if (!activeDevices.Contains(inputDevice))
        {
            Debug.Log("Player joins");
            LobbyPreferences.AddDevice(inputDevice);
        }
    }
}
