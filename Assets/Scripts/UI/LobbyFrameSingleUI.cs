using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyFrameSingleUI : MonoBehaviour
{
    public class FrameData
    {
        public InputDevice inputDevice;
        public Color color;
        public Sprite characterSprite;
    }
    
    [Header("References")]
    [SerializeField] private GameObject passiveStateWithEnterGameObjects;
    [SerializeField] private GameObject passiveStateWithoutEnterGameObjects;
    [SerializeField] private GameObject activeStateGameObjects;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject readyIcon;

    private FrameData frameData;
    private bool isActive;
    
    private void Start()
    {
        passiveStateWithEnterGameObjects.SetActive(true);
        activeStateGameObjects.SetActive(false);
        DisableReadyVisual();
    }

    public void EnableActiveState(FrameData newFrameData)
    {
        isActive = true;
        frameData = newFrameData;
        
        backgroundImage.color = frameData.color;
        characterImage.sprite = frameData.characterSprite;

        activeStateGameObjects.SetActive(true);
        
        passiveStateWithEnterGameObjects.SetActive(false);
        passiveStateWithoutEnterGameObjects.SetActive(false);
    }

    public void EnablePassiveState()
    {
        frameData = null;
        isActive = false;

        // if (LobbyManager.Instance.IsKeyboardRegistered())
        // {
        //     EnablePassiveStateWithoutKeyboardIcon();
        // }
        // else
        // {
        //     EnablePassiveStateWithKeyboardIcon();
        // }
        
        UpdatePassiveState();
    }

    public void ChangeTeam(TeamColorsSO teamColorsSO)
    {
        frameData.color = teamColorsSO.GetNextColor(frameData.color);
        backgroundImage.color = frameData.color;
    }

    public void SwitchToNextSkin(CharacterDataSO characterDataSO)
    {
        frameData.characterSprite = characterDataSO.GetNextCharacterSprite(characterImage.sprite);
        characterImage.sprite = frameData.characterSprite;
    }

    public void SwitchToPreviousSkin(CharacterDataSO characterDataSO)
    {
        frameData.characterSprite = characterDataSO.GetPreviousCharacterSprite(characterImage.sprite);
        characterImage.sprite = frameData.characterSprite;
    }

    public bool IsValidFrame(InputDevice inputDevice)
    {
        return frameData.inputDevice == inputDevice;
    }

    public bool IsActive()
    {
        return frameData != null;
    }

    public void EnableReadyVisual()
    {
        readyIcon.SetActive(true);
    }
    
    public void DisableReadyVisual()
    {
        readyIcon.SetActive(false);
    }

    public FrameData GetFrameData()
    {
        return frameData;
    }

    // public void EnablePassiveStateWithKeyboardIcon()
    // {
    //     if (isActive)
    //     {
    //         return;
    //     }
    //
    //     passiveStateWithEnterGameObjects.SetActive(true);
    //     
    //     passiveStateWithoutEnterGameObjects.SetActive(false);
    //     activeStateGameObjects.SetActive(false);
    // }
    //
    // public void EnablePassiveStateWithoutKeyboardIcon()
    // {
    //     if (isActive)
    //     {
    //         return;
    //     }
    //
    //     passiveStateWithoutEnterGameObjects.SetActive(true);
    //     
    //     passiveStateWithEnterGameObjects.SetActive(false);
    //     activeStateGameObjects.SetActive(false);
    // }

    public void UpdatePassiveState()
    {
        if (isActive)
        {
            return;
        }
        
        activeStateGameObjects.SetActive(false);
        
        if (LobbyManager.Instance.IsKeyboardRegistered())
        {
            passiveStateWithEnterGameObjects.SetActive(false);
            passiveStateWithoutEnterGameObjects.SetActive(true);
        }
        else
        {
            passiveStateWithoutEnterGameObjects.SetActive(false);
            passiveStateWithEnterGameObjects.SetActive(true);
        }
    }
}
