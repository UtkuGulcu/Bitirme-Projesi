using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private TMP_Text winnerTeamText;

    private bool hasGameEnded;

    private void Start()
    {
        DisableVisuals();
    }

    private void Update()
    {
        if (!hasGameEnded)
        {
            return;
        }
        
        if (Gamepad.current.buttonEast.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Time.timeScale = 1;
            SceneLoader.LoadMainMenu();
        }
    }

    public void ShowEndGameScreen(object sender, object data)
    {
        string winnerTeamName = data as string;
        winnerTeamName = winnerTeamName.ToUpper();
        
        winnerTeamText.text = $"{winnerTeamName} TEAM";
        
        EnableVisuals();
        
        hasGameEnded = true;
        Time.timeScale = 0f;
    }
    
    private void EnableVisuals()
    {
        visuals.SetActive(true);
    }

    private void DisableVisuals()
    {
        visuals.SetActive(false);
    }
}
