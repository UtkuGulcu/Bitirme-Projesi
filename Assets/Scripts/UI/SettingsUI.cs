using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject visual;
    [SerializeField] private Image soundImage;
    [SerializeField] private Image musicImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    private bool isSettingsActive;
    private bool isSoundOff;
    private bool isMusicOff;
    
    public void EnableVisual()
    {
        isSettingsActive = true;
        visual.SetActive(true);
    }

    private void DisableVisual()
    {
        isSettingsActive = false;
        visual.SetActive(false);
    }

    private void Update()
    {
        if (!isSettingsActive)
        {
            return;
        }
        
        if (Gamepad.current?.buttonWest.wasPressedThisFrame ?? Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ToggleSound();
        }

        if (Gamepad.current?.buttonNorth.wasPressedThisFrame ?? Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleMusic();
        }
        
        if (Gamepad.current?.buttonEast.wasPressedThisFrame ?? Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DisableVisual();
        }
    }

    private void ToggleMusic()
    {
        isMusicOff = !isMusicOff;
        musicImage.sprite = isMusicOff ? musicOffSprite : musicOnSprite;
        SoundManager.Instance.MuteMusic(isMusicOff);
    }
    
    private void ToggleSound()
    {
        isSoundOff = !isSoundOff;
        soundImage.sprite = isSoundOff ? soundOffSprite : soundOnSprite;
        SoundManager.Instance.MuteSound(isSoundOff);
    }
    
}
