using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public struct PlayerData
    {
        public InputDevice inputDevice;
        public int playerID;
        public GameObject prefab;
        public LobbyPreferences.PlayerPreferences.Team team;
        public int health;
    }
    
    public static PlayerManager Instance { get; private set; }
    
    
    [Header("References")]
    [SerializeField] private GameObject[] playerPrefabArray;
    [SerializeField] private Transform[] spawnPositionTransformArray;

    private Dictionary<int, PlayerData> playerDataDictionary;
    private int currentID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are multiple PlayerManagers!");
            Destroy(this);
        }
        
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        playerDataDictionary = new Dictionary<int, PlayerData>();
        
        foreach (var playerPreference in LobbyPreferences.GetPlayerPreferencesList())
        {
            currentID++;

            var playerData = new PlayerData
            {
                inputDevice = playerPreference.inputDevice,
                playerID = currentID,
                prefab = playerPreference.playerPrefab,
                team = playerPreference.team,
                health = 3
            };
            
            playerDataDictionary.Add(currentID, playerData);
        }
        
        SpawnAllPlayers();
    }

    private void SpawnAllPlayers()
    {
        foreach (var playerData in playerDataDictionary.Values)
        {
            SpawnPlayerWithID(playerData);
        }
    }
    
    public void OnPlayerDied(object sender, object data)
    {
        int deadPlayerID = (int)data;
        
        PlayerData tempData = playerDataDictionary[deadPlayerID];
        tempData.health--;
        playerDataDictionary[deadPlayerID] = tempData;
        
        PlayerInformationUI.Instance.UpdatePanels();
        
        if (CanPlayerSpawn(deadPlayerID))
        {
            StartCoroutine(WaitToSpawn(deadPlayerID));
        }
    }

    private IEnumerator WaitToSpawn(int deadPlayerID)
    {
        yield return Helpers.GetWait(1);

        PlayerData playerData = playerDataDictionary[deadPlayerID];
        SpawnPlayerWithID(playerData);
    }
    
    private void SpawnPlayerWithID(PlayerData playerData)
    {
        //GameObject selectedPrefab = playerPrefabArray[playerData.playerID - 1];
        GameObject selectedPrefab = playerData.prefab;
        Transform selectedSpawnPositionTransform = spawnPositionTransformArray[playerData.playerID - 1];
        
        GameObject spawnedObject = Instantiate(selectedPrefab, selectedSpawnPositionTransform.position, Quaternion.identity);

        PlayerInput playerInput = spawnedObject.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentControlScheme(playerData.inputDevice);

        Player player = spawnedObject.GetComponent<Player>();
        player.SetPlayerData(playerData.playerID, playerData.team);
    }

    private bool CanPlayerSpawn(int ID)
    {
        return playerDataDictionary[ID].health > 0;
    }

    public List<PlayerData> GetAllPlayerInformation()
    {
        return playerDataDictionary.Values.ToList();
    }

    // public void UpdateWeaponNameWithID(int playerID, string weaponName)
    // {
    //     PlayerData tempData = playerDataDictionary[playerID];
    //     tempData.weaponName = weaponName;
    //     playerDataDictionary[playerID] = tempData;
    //     PlayerInformationUI.Instance.UpdatePanels();
    // }
}