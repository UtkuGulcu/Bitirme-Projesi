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
    [SerializeField] private GameEventSO OnPlayerSetReady;
    [SerializeField] private GameEventSO OnPlayerSetNotReady;
    [SerializeField] private GameEventSO OnPlayerLeft;
    
    [Header("References")]
    [SerializeField] private CharacterDataSO characterData;
    
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

    private void Start()
    {
        LobbyPreferences.ClearMemory();
    }

    private void Update()
    {
        ReadKeyboardInput();
        ReadGamepadInput();
    }

    private void ReadKeyboardInput()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current);
            HandleSubmitKeyInput(isJoined, Keyboard.current);
        }

        if (Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current, true);
            HandleSubmitKeyInput(isJoined, Keyboard.current, true);
        }
        
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current);
            HandleCancelKeyInput(isJoined, Keyboard.current);
        }
        
        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current, true);
            HandleCancelKeyInput(isJoined, Keyboard.current, true);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current);
            HandleChangeTeamInput(isJoined, Keyboard.current);
        }

        if (Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current, true);
            HandleChangeTeamInput(isJoined, Keyboard.current, true);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current);
            HandleChangeSkinInput(isJoined,Keyboard.current, true);
        }
        
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current);
            HandleChangeSkinInput(isJoined, Keyboard.current, false);
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current, true);
            HandleChangeSkinInput(isJoined, Keyboard.current, true, true);
        }
        
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(Keyboard.current, true);
            HandleChangeSkinInput(isJoined, Keyboard.current, false, true);
        }
    }

    private void ReadGamepadInput()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad == null)
        {
            return;
        }

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(gamepad);
            HandleSubmitKeyInput(isJoined, gamepad);
        }
        
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(gamepad);
            HandleCancelKeyInput(isJoined, gamepad);
        }

        if (gamepad.buttonWest.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(gamepad);
            HandleChangeTeamInput(isJoined, gamepad);
        }

        if (gamepad.leftStick.left.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(gamepad);
            HandleChangeSkinInput(isJoined, gamepad, false);
        }

        if (gamepad.leftStick.right.wasPressedThisFrame)
        {
            bool isJoined = LobbyPreferences.IsDeviceRegistered(gamepad);
            HandleChangeSkinInput(isJoined, gamepad, true);
        }
    }

    private void HandleSubmitKeyInput(bool isJoined, InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        switch (isJoined)
        {
            case false when CanJoin():
                LobbyPreferences.AddDevice(inputDevice, characterData.GetDefaultSkinPrefab(), characterData.GetDefaultPortraitSprite(), isSecondKeyboard);
                
                var onPlayerJoinedEventArgs = GameEventArgs.GetInputDeviceEventArgs(inputDevice, isSecondKeyboard);
                OnPlayerJoined.Raise(this, onPlayerJoinedEventArgs);
                break;
            
            case true:
                LobbyPreferences.SetPlayerReady(inputDevice, isSecondKeyboard);
                
                var onPlayerSetReadyEventArgs = GameEventArgs.GetInputDeviceEventArgs(inputDevice, isSecondKeyboard);
                OnPlayerSetReady.Raise(this, onPlayerSetReadyEventArgs);
                break;
        }
    }

    private void HandleCancelKeyInput(bool isJoined, InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        switch (isJoined)
        {
            case false:
                SceneLoader.LoadMainMenu();
                break;
            
            case true when LobbyPreferences.IsPlayerReady(inputDevice, isSecondKeyboard):
                StartCoroutine(LobbyPreferences.SetPlayerNotReady(inputDevice));
                
                var onPlayerSetNotReadyEventArgs = GameEventArgs.GetInputDeviceEventArgs(inputDevice, isSecondKeyboard);
                OnPlayerSetNotReady.Raise(this, onPlayerSetNotReadyEventArgs);
                break;
            
            case true when !LobbyPreferences.IsPlayerReady(inputDevice, isSecondKeyboard):
                LobbyPreferences.DeletePlayerPreferences(inputDevice, isSecondKeyboard);
                
                var onPlayerLeftEventArgs = GameEventArgs.GetInputDeviceEventArgs(inputDevice, isSecondKeyboard);
                OnPlayerLeft.Raise(this, onPlayerLeftEventArgs);
                break;
        }
    }

    private void HandleChangeTeamInput(bool isJoined, InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        if (!isJoined)
        {
            return;
        }
        
        LobbyPreferences.ChangeTeamOfPlayer(inputDevice, isSecondKeyboard);
        
        var onPlayerChangedTeamEventArgs = GameEventArgs.GetInputDeviceEventArgs(inputDevice, isSecondKeyboard);
        OnPlayerChangedTeam.Raise(this, onPlayerChangedTeamEventArgs);
    }

    private void HandleChangeSkinInput(bool isJoined, InputDevice inputDevice, bool hasSwitchedToNextSkin, bool isSecondKeyboard = false)
    {
        if (!isJoined)
        {
            return;
        }
        
        GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(inputDevice, isSecondKeyboard);
        Sprite currentSprite = LobbyPreferences.GetPortraitOfPlayer(inputDevice, isSecondKeyboard);

        GameObject newPrefab;
        Sprite newSprite;
        
        if (hasSwitchedToNextSkin)
        {
            newPrefab = characterData.GetNextPrefab(currentPrefab);
            newSprite = characterData.GetNextPortraitSprite(currentSprite);
        }
        else
        {
            newPrefab = characterData.GetPreviousPrefab(currentPrefab);
            newSprite = characterData.GetPreviousPortraitSprite(currentSprite);
        }
        
        LobbyPreferences.ChangeSkinOfPlayer(inputDevice, newPrefab, newSprite, isSecondKeyboard);

        var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(hasSwitchedToNextSkin, inputDevice, isSecondKeyboard);
        OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
    }
    
    
    private bool CanJoin()
    {
        return LobbyPreferences.GetPlayerCount() < 4;
    }
}
