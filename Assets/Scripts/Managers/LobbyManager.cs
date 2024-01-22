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
        
        var gamepad = Gamepad.current;

        if (gamepad == null)
        {
            return;
        }

        if (IsGamepadRegistered(gamepad))
        {
            ReadRegisteredGamepadInput();
        }
        else
        {
            ReadUnregisteredGamepadInput();
        }
    }

    private void ReadUnregisteredKeyboardInput()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneLoader.LoadMainMenu();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame && CanJoin() && LobbyPreferences.TryToAddDevice(Keyboard.current, characterData.GetDefaultSkinPrefab(), characterData.GetDefaultPortraitSprite()))
        {
            keyboard = Keyboard.current;
            OnPlayerJoined.Raise(this, keyboard);
        }
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
            Sprite currentSprite = LobbyPreferences.GetPortraitOfPlayer(keyboard);

            GameObject nextPrefab = characterData.GetNextPrefab(currentPrefab);
            Sprite nextSprite = characterData.GetNextPortraitSprite(currentSprite);
            
            LobbyPreferences.ChangeSkinOfPlayer(keyboard, nextPrefab, nextSprite);

            var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(true, keyboard);
            OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
        }

        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(keyboard);
            Sprite currentSprite = LobbyPreferences.GetPortraitOfPlayer(keyboard);
            
            GameObject previousPrefab = characterData.GetPreviousPrefab(currentPrefab);
            Sprite previousSprite = characterData.GetPreviousPortraitSprite(currentSprite);
            
            LobbyPreferences.ChangeSkinOfPlayer(keyboard, previousPrefab, previousSprite);
                
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
            var tempKeyboard = keyboard;
            keyboard = null;
            
            LobbyPreferences.DeletePlayerPreferences(tempKeyboard);
            OnPlayerLeft.Raise(this, tempKeyboard);
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
                Sprite currentSprite = LobbyPreferences.GetPortraitOfPlayer(gamepad);

                GameObject nextPrefab = characterData.GetNextPrefab(currentPrefab);
                Sprite nextSprite = characterData.GetNextPortraitSprite(currentSprite);
            
                LobbyPreferences.ChangeSkinOfPlayer(gamepad, nextPrefab, nextSprite);

                var onPlayerChangedSkinEventArgs = GameEventArgs.GetOnPlayerChangedSkinEventArgs(true, gamepad);
                OnPlayerChangedSkin.Raise(this,onPlayerChangedSkinEventArgs);
            }
            
            if (gamepad.leftStick.left.wasPressedThisFrame)
            {
                GameObject currentPrefab = LobbyPreferences.GetPrefabOfPlayer(gamepad);
                Sprite currentSprite = LobbyPreferences.GetPortraitOfPlayer(gamepad);
            
                GameObject previousPrefab = characterData.GetPreviousPrefab(currentPrefab);
                Sprite previousSprite = characterData.GetPreviousPortraitSprite(currentSprite);
            
                LobbyPreferences.ChangeSkinOfPlayer(gamepad, previousPrefab, previousSprite);
                
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
                StartCoroutine(LobbyPreferences.SetPlayerNotReady(gamepad));
                OnPlayerSetNotReady.Raise(this, gamepad);
            }
            
            if (gamepad.buttonEast.wasPressedThisFrame && !LobbyPreferences.IsPlayerReady(gamepad))
            {
                OnPlayerLeft.Raise(this, gamepad);
                LobbyPreferences.DeletePlayerPreferences(gamepad);
                joinedGamepadList.Remove(gamepad);
            }
        }
    }

    private void ReadUnregisteredGamepadInput()
    {
        var gamepad = Gamepad.current;

        if (gamepad == null || IsGamepadRegistered(gamepad))
        {
            return;
        }
        
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            SceneLoader.LoadMainMenu();
        }

        if (gamepad.buttonSouth.wasPressedThisFrame && CanJoin() && LobbyPreferences.TryToAddDevice(gamepad, characterData.GetDefaultSkinPrefab(), characterData.GetDefaultPortraitSprite()))
        {
            joinedGamepadList.Add(gamepad);
            OnPlayerJoined.Raise(this, gamepad);
        }
    }

    private bool CanJoin()
    {
        bool canJoin = (keyboard == null && joinedGamepadList.Count < 4) ||
                       (keyboard != null && joinedGamepadList.Count < 3);

        return canJoin;
    }
    
    private bool IsGamepadRegistered(Gamepad gamepad)
    {
        return joinedGamepadList.Contains(gamepad);
    }

    public bool IsKeyboardRegistered()
    {
        return keyboard != null;
    }
}
