using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LobbyFrameSingleUI[] frameArray;
    [SerializeField] private TeamColorsSO teamColorsSO;
    [SerializeField] private CharacterDataSO characterDataSO;

    private int nextAvailableIndex;
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

    // private void EnablePassiveStateWithKeyboardOfFrames()
    // {
    //     foreach (var frame in frameArray)
    //     {
    //         frame.EnablePassiveStateWithKeyboardIcon();
    //     }
    // }
    //
    // private void EnablePassiveStateWithoutKeyboardOfFrames()
    // {
    //     foreach (var frame in frameArray)
    //     {
    //         frame.EnablePassiveStateWithoutKeyboardIcon();
    //     }
    // }

    private void UpdatePassiveStateOfFrames()
    {
        foreach (var frame in frameArray)
        {
            frame.UpdatePassiveState();
        }
    }
}
