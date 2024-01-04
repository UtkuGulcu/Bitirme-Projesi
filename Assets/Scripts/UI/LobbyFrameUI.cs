using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyFrameUI : MonoBehaviour
{
    public class FrameData
    {
        public InputDevice inputDevice;
        public Color color;
        public Sprite characterSprite;
    }
    
    [Header("References")]
    [SerializeField] private GameObject passiveStateGameObjects;
    [SerializeField] private GameObject activeStateGameObjects;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject readyIcon;

    private FrameData frameData;
    
    private void Start()
    {
        passiveStateGameObjects.SetActive(true);
        activeStateGameObjects.SetActive(false);
        DisableReadyVisual();
    }

    public void EnableActiveState(FrameData newFrameData)
    {
        frameData = newFrameData;
        
        backgroundImage.color = frameData.color;
        
        characterImage.sprite = frameData.characterSprite;

        passiveStateGameObjects.SetActive(false);
        activeStateGameObjects.SetActive(true);
    }

    public void EnablePassiveState()
    {
        frameData = null;
        passiveStateGameObjects.SetActive(true);
        activeStateGameObjects.SetActive(false);
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
}
