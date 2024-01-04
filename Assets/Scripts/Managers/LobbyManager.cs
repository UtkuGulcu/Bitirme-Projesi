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

    private void Start()
    {
        LobbyPreferences.ClearMemory();
    }

    private void Update()
    {
        if (keyboard == null)
        {
            ReadUnregisteredKeyboardInput();
        }
        else
        {
            ReadRegisteredKeyboardInput();
        }

        ReadRegisteredGamepadInput();
    }

    private void ReadUnregisteredKeyboardInput()
    {
        // if (Keyboard.current.escapeKey)
        // {
        //     
        // }
    }

    private void ReadRegisteredKeyboardInput()
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

        if (keyboard.enterKey.wasPressedThisFrame)
        {
            LobbyPreferences.SetPlayerReady(keyboard);
            OnPlayerSetReady.Raise(this, keyboard);
        }

        if (keyboard.escapeKey.wasPressedThisFrame && LobbyPreferences.IsPlayerReady(keyboard))
        {
            StartCoroutine(LobbyPreferences.SetPlayerNotReady(keyboard));
            OnPlayerSetNotReady.Raise(this, keyboard);
        }

        if (keyboard.escapeKey.wasPressedThisFrame && !LobbyPreferences.IsPlayerReady(keyboard))
        {
            LobbyPreferences.DeletePlayerPreferences(keyboard);
            OnPlayerLeft.Raise(this, keyboard);
            keyboard = null;
        }
    }

    private void ReadRegisteredGamepadInput()
    {
        for (int i = joinedGamepadList.Count - 1; i >= 0; i--)
        {
            Gamepad gamepad = joinedGamepadList[i];
            
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

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                LobbyPreferences.SetPlayerReady(gamepad);
                OnPlayerSetReady.Raise(this, gamepad);
            }

            if (gamepad.buttonEast.wasPressedThisFrame && LobbyPreferences.IsPlayerReady(gamepad))
            {
                Debug.Log("Circle ready");
                StartCoroutine(LobbyPreferences.SetPlayerNotReady(gamepad));
                OnPlayerSetNotReady.Raise(this, gamepad);
            }
            
            if (gamepad.buttonEast.wasPressedThisFrame && !LobbyPreferences.IsPlayerReady(gamepad))
            {
                Debug.Log("Circle not ready");
                OnPlayerLeft.Raise(this, gamepad);
                LobbyPreferences.DeletePlayerPreferences(gamepad);
                joinedGamepadList.Remove(gamepad);
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
                StartCoroutine(AddGamepadToList(gamepad));
                OnPlayerJoined.Raise(this, gamepad);
                break;
            
            case Keyboard keyboardDevice when keyboardDevice.enterKey.wasPressedThisFrame && LobbyPreferences.TryToAddDevice(keyboardDevice, characterData.GetDefaultSkinPrefab()):
                StartCoroutine(SetKeyboard(keyboardDevice));
                OnPlayerJoined.Raise(this, keyboardDevice);
                break;
        }
    }

    private bool CanJoin()
    {
        bool canJoin = (keyboard == null && joinedGamepadList.Count < 4) ||
                       (keyboard != null && joinedGamepadList.Count < 3);

        return canJoin;
    }

    private IEnumerator AddGamepadToList(Gamepad gamepad)
    {
        yield return new WaitForEndOfFrame();
        joinedGamepadList.Add(gamepad);
    }
    
    private IEnumerator SetKeyboard(Keyboard keyboard)
    {
        yield return new WaitForEndOfFrame();
        this.keyboard = keyboard;
    }

    private bool IsGamepadRegistered(Gamepad gamepad)
    {
        return joinedGamepadList.Contains(gamepad);
    }
}
