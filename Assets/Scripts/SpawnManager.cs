using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        foreach (InputDevice inputDevice in LobbyPreferences.GetActiveDevices())
        {
            GameObject spawnedObject = Instantiate(playerPrefab);
            PlayerInput playerInput = spawnedObject.GetComponent<PlayerInput>();
            playerInput.SwitchCurrentControlScheme(inputDevice);
        }
    }
}
