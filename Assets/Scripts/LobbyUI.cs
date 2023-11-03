using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonCLicked);
    }

    private void OnStartGameButtonCLicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
