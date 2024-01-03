using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyFrameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject passiveStateGameObjects;
    [SerializeField] private GameObject activeStateGameObjects;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image characterImage;
    //[SerializeField] private TeamColorsSO teamColorsSO;


    private InputDevice inputDevice;
    private Color currentColor;
    
    private void Start()
    {
        passiveStateGameObjects.SetActive(true);
        activeStateGameObjects.SetActive(false);
    }

    public void EnableActiveState(InputDevice inputDevice, Color defaultTeamColor, Sprite defaultCharacterSprite)
    {
        this.inputDevice = inputDevice;
        currentColor = defaultTeamColor;
        backgroundImage.color = currentColor;
        
        characterImage.sprite = defaultCharacterSprite;

        passiveStateGameObjects.SetActive(false);
        activeStateGameObjects.SetActive(true);
    }

    public void ChangeTeam(TeamColorsSO teamColorsSO)
    {
        currentColor = teamColorsSO.GetNextColor(currentColor);
        backgroundImage.color = currentColor;
    }

    public void SwitchToNextSkin(CharacterDataSO characterDataSO)
    {
        characterImage.sprite = characterDataSO.GetNextCharacterSprite(characterImage.sprite);
    }

    public void SwitchToPreviousSkin(CharacterDataSO characterDataSO)
    {
        characterImage.sprite = characterDataSO.GetPreviousCharacterSprite(characterImage.sprite);
    }

    public bool IsValidFrame(InputDevice inputDevice)
    {
        return this.inputDevice == inputDevice;
    }

    public Color GetCurrentColor()
    {
        return currentColor;
    }
}
