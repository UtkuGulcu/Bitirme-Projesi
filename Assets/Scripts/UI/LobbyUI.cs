using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public enum Level
    {
        Arena,
        Space
    }
    
    public static LobbyUI Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private LobbyFrameSingleUI[] frameArray;
    [SerializeField] private TeamColorsSO teamColorsSO;
    [SerializeField] private CharacterDataSO characterDataSO;
    [SerializeField] private Image mapFrameImage;
    [SerializeField] private Sprite arenaLevelSprite;
    [SerializeField] private Sprite spaceLevelSprite;

    private int nextAvailableIndex;
    private Level selectedLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are more than one LobbyUIs");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            ChangeMapFrame();
        }
    }

    public void EnableFrame(object sender, object data)
    {
        var inputDevice = data as InputDevice;

        var frameData = new LobbyFrameSingleUI.FrameData
        {
            inputDevice = inputDevice,
            color = teamColorsSO.GetDefaultColor(),
            characterSprite = characterDataSO.GetDefaultCharacterSprite()
        };
        
        frameArray[nextAvailableIndex].EnableActiveState(frameData);
        nextAvailableIndex++;
        
        if (inputDevice is Keyboard)
        {
            UpdatePassiveStateOfFrames();
        }
    }

    public void DisableFrame(object sender, object data)
    {
        nextAvailableIndex--;
        var inputDevice = data as InputDevice;
        
        if (inputDevice is Keyboard)
        {
            UpdatePassiveStateOfFrames();
        }
        
        int disabledFrameIndex = FindIndexOfFrame(inputDevice);

        for (int i = 0; i < frameArray.Length; i++)
        {
            if (i < disabledFrameIndex)
            {
                continue;
            }

            if (i == disabledFrameIndex)
            {
                frameArray[i].EnablePassiveState();

                if (!frameArray[i + 1].IsActive())
                {
                    return;
                }
                
                continue;
            }

            var frameData = frameArray[i].GetFrameData();
            frameArray[i - 1].EnableActiveState(frameData);
            frameArray[i].EnablePassiveState();

            if (i == frameArray.Length - 1)
            {
                return;
            }
            
            if (!frameArray[i + 1].IsActive())
            {
                return;
            }
        }
    }

    public void ChangeTeamOfFrame(object sender, object data)
    {
        LobbyFrameSingleUI frameSingleWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameSingleWithInputDevice.ChangeTeam(teamColorsSO);
    }

    public void ChangeSkinOfFrame(object sender, object data)
    {
        var args = data as GameEventArgs.OnPlayerChangedSkinEventArgs;
        LobbyFrameSingleUI frameSingleWithInputDevice = FindFrameWithInputDevice(args.inputDevice);

        if (args.hasSwitchedToNextSkin)
        {
            frameSingleWithInputDevice.SwitchToNextSkin(characterDataSO);
        }
        else
        {
            frameSingleWithInputDevice.SwitchToPreviousSkin(characterDataSO);
        }
        
    }

    public void SetFrameReady(object sender, object data)
    {
        LobbyFrameSingleUI frameSingleWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameSingleWithInputDevice.EnableReadyVisual();
    }
    
    public void SetFrameNotReady(object sender, object data)
    {
        LobbyFrameSingleUI frameSingleWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameSingleWithInputDevice.DisableReadyVisual();
    }

    private LobbyFrameSingleUI FindFrameWithInputDevice(InputDevice inputDevice)
    {
        LobbyFrameSingleUI tempFrameSingle = null;
        
        foreach (var frame in frameArray)
        {
            if (frame.IsValidFrame(inputDevice))
            {
                tempFrameSingle = frame;
                break;
            }
        }

        return tempFrameSingle;
    }

    private int FindIndexOfFrame(InputDevice inputDevice)
    {
        foreach (var frame in frameArray)
        {
            if (frame.IsValidFrame(inputDevice))
            {
                return Array.IndexOf(frameArray, frame);
            }
        }

        return -1;
    }

    private void UpdatePassiveStateOfFrames()
    {
        foreach (var frame in frameArray)
        {
            frame.UpdatePassiveState();
        }
    }
    
    private void ChangeMapFrame()
    {
        Level[] values = (Level[])Enum.GetValues(typeof(Level));
        int currentIndex = Array.IndexOf(values, selectedLevel);
            
        Level nextLevel = currentIndex < values.Length - 1 ? values[currentIndex + 1] : values[0];
        selectedLevel = nextLevel;

        mapFrameImage.sprite = selectedLevel == Level.Arena ? arenaLevelSprite : spaceLevelSprite;
    }

    public Level GetSelectedLevel()
    {
        return selectedLevel;
    }
}
