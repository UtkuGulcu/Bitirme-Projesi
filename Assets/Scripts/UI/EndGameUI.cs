using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject visuals;
    [SerializeField] private TMP_Text winnerTeamText;
    [SerializeField] private TeamColorsSO teamColorsSO;
    [SerializeField] private BaseUIPanelAnimation baseUIPanelAnimation;

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
        
        if (Gamepad.current?.buttonEast.wasPressedThisFrame ?? Keyboard.current.escapeKey.wasPressedThisFrame)
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
        SetTeamColor(winnerTeamName);
        
        EnableVisuals();
        baseUIPanelAnimation.PlayOpenAnimation();
        
        hasGameEnded = true;
        Time.timeScale = 0f;
    }

    private void SetTeamColor(string winnerTeamName)
    {
        foreach (var teamColor in teamColorsSO.teamColorArray)
        {
            if (string.Equals(teamColor.teamName, winnerTeamName, StringComparison.CurrentCultureIgnoreCase))
            {
                winnerTeamText.color = teamColor.color;
            }
        }
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
