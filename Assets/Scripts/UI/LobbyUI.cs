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
        frameArray[nextAvailableIndex].EnableActiveState(inputDevice, teamColorsSO.GetDefaultColor(), characterDataSO.GetDefaultCharacterSprite());
        nextAvailableIndex++;
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
}
