using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEventSO OnPlayerJoined;
    [SerializeField] private GameEventSO OnPlayerChangedTeam;
    [SerializeField] private GameEventSO OnPlayerChangedSkin;
    
    [Header("References")]
    [SerializeField] private CharacterDataSO characterData;
    
    public static LobbyManager Instance { get; private set; }

    private Keyboard keyboard;
    private List<Gamepad> joinedGamepadList = new();

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

    private void Update()
    {
        if (keyboard != null)
        {
            ReadKeyboardInput();
        }

        ReadGamepadInput();
    }

    private void ReadKeyboardInput()
    {
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            LobbyPreferences.ChangeTeamOfPlayer(keyboard);
            OnPlayerChangedTeam.Raise(this, keyboard);
        }

        if (keyboard.rightArrowKey.wasPressedThisFrame)
        {
            GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(keyboard);
            GameObject nextPrefab = characterData.GetNextPrefab(currentPrefab);
            LobbyPreferences.ChangeSkinOfPlayer(keyboard, nextPrefab);

            var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(true, keyboard);
            OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
        }

        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(keyboard);
            GameObject previousPrefab = characterData.GetPreviousPrefab(currentPrefab);
            LobbyPreferences.ChangeSkinOfPlayer(keyboard, previousPrefab);
                
            var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(false, keyboard);
            OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
        }
    }

    private void ReadGamepadInput()
    {
        foreach (var gamepad in joinedGamepadList)
        {
            if (gamepad.buttonWest.wasPressedThisFrame)
            {
                LobbyPreferences.ChangeTeamOfPlayer(gamepad);
                OnPlayerChangedTeam.Raise(this, gamepad);
            }

            if (gamepad.leftStick.right.wasPressedThisFrame)
            {
                GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(gamepad);
                GameObject nextPrefab = characterData.GetNextPrefab(currentPrefab);
                LobbyPreferences.ChangeSkinOfPlayer(gamepad, nextPrefab);

                var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(true, gamepad);
                OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
            }
            
            if (gamepad.leftStick.left.wasPressedThisFrame)
            {
                GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(gamepad);
                GameObject previousPrefab = characterData.GetPreviousPrefab(currentPrefab);
                LobbyPreferences.ChangeSkinOfPlayer(gamepad, previousPrefab);
                
                var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(false, gamepad);
                OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
            }
        }
    }

    private void InputSystem_OnActionChange(object arg1, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed)
        {
            return;
        }

        InputAction inputAction = arg1 as InputAction;
        InputDevice inputDevice = inputAction.activeControl.device;

        if (inputDevice.name == "Mouse")
        {
            return;
        }

        if (!CanJoin())
        {
            return;
        }

        switch (inputDevice)
        {
            case Gamepad gamepad when gamepad.buttonSouth.wasPressedThisFrame && LobbyPreferences.TryToAddDevice(inputDevice, characterData.GetDefaultSkinPrefab()):
                joinedGamepadList.Add(gamepad);
                OnPlayerJoined.Raise(this, gamepad);
                break;
            
            case Keyboard keyboardDevice when keyboardDevice.enterKey.wasPressedThisFrame && LobbyPreferences.TryToAddDevice(keyboardDevice, characterData.GetDefaultSkinPrefab()):
                keyboard = keyboardDevice;
                OnPlayerJoined.Raise(this, keyboard);
                break;
        }
    }

    private bool CanJoin()
    {
        bool canJoin = (keyboard == null && joinedGamepadList.Count < 4) ||
                       (keyboard != null && joinedGamepadList.Count < 3);

        return canJoin;
    }
}
