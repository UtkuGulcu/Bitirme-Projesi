using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LobbyFrameUI[] frameArray;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TeamColorsSO teamColorsSO;
    [SerializeField] private CharacterDataSO characterDataSO;

    private int nextAvailableIndex;

    private void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    private void OnStartGameButtonClicked()
    {
        SceneLoader.LoadNextScene();
    }

    public void EnableFrame(object sender, object data)
    {
        var inputDevice = data as InputDevice;

        var frameData = new LobbyFrameUI.FrameData
        {
            inputDevice = inputDevice,
            color = teamColorsSO.GetDefaultColor(),
            characterSprite = characterDataSO.GetDefaultCharacterSprite()
        };
        
        frameArray[nextAvailableIndex].EnableActiveState(frameData);
        nextAvailableIndex++;
    }

    public void DisableFrame(object sender, object data)
    {
        nextAvailableIndex--;
        var inputDevice = data as InputDevice;
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
        LobbyFrameUI frameWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameWithInputDevice.ChangeTeam(teamColorsSO);
    }

    public void ChangeSkinOfFrame(object sender, object data)
    {
        var args = data as GameEventArgs.OnPlayerChangedSkinEventArgs;
        LobbyFrameUI frameWithInputDevice = FindFrameWithInputDevice(args.inputDevice);

        if (args.hasSwitchedToNextSkin)
        {
            frameWithInputDevice.SwitchToNextSkin(characterDataSO);
        }
        else
        {
            frameWithInputDevice.SwitchToPreviousSkin(characterDataSO);
        }
        
    }

    public void SetFrameReady(object sender, object data)
    {
        LobbyFrameUI frameWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameWithInputDevice.EnableReadyVisual();
    }
    
    public void SetFrameNotReady(object sender, object data)
    {
        LobbyFrameUI frameWithInputDevice = FindFrameWithInputDevice(data as InputDevice);
        frameWithInputDevice.DisableReadyVisual();
    }

    private LobbyFrameUI FindFrameWithInputDevice(InputDevice inputDevice)
    {
        LobbyFrameUI tempFrame = null;
        
        foreach (var frame in frameArray)
        {
            if (frame.IsValidFrame(inputDevice))
            {
                tempFrame = frame;
                break;
            }
        }

        return tempFrame;
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
}
