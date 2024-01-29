using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameEventSO OnMainMenuMusicPlayed;
    [SerializeField] private SettingsUI settingsUI;
    
    private void Start()
    {
        SetupCursor();
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        OnMainMenuMusicPlayed.Raise();
    }

    private void OnSettingsButtonClicked()
    {
        settingsUI.EnableVisual();
    }

    private void OnStartButtonClicked()
    {
        SceneLoader.LoadNextScene();
    }
    
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void SetupCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
