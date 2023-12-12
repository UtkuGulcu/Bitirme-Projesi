using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerManagerPrefab;

    // private void Awake()
    // {
    //     foreach (InputDevice inputDevice in LobbyPreferences.GetActiveDevices())
    //     {
    //         // GameObject spawnedObject = Instantiate(playerManager);
    //         // PlayerInput playerInput = spawnedObject.GetComponent<PlayerInput>();
    //         // playerInput.SwitchCurrentControlScheme(inputDevice);
    //         
    //         PlayerManager playerManager = Instantiate(playerManagerPrefab).GetComponent<PlayerManager>();
    //         playerManager.SpawnPlayer(inputDevice);
    //     }
    // }
}
