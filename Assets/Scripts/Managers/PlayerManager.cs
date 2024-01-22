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
        public Color teamColor;
        public Sprite portraitSprite;
        public int health;
    }
    
    public static PlayerManager Instance { get; private set; }

    [Header("Events")]
    [SerializeField] private GameEventSO OnGameEnded;
    
    [Header("References")]
    [SerializeField] private Transform[] spawnPositionTransformArray;
    [SerializeField] private TeamColorsSO teamColorsSO;

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
            var teamColor = teamColorsSO.GetColorWithName(playerPreference.team.ToString());
            
            var playerData = new PlayerData
            {
                inputDevice = playerPreference.inputDevice,
                playerID = currentID,
                prefab = playerPreference.playerPrefab,
                team = playerPreference.team,
                teamColor = teamColor,
                portraitSprite = playerPreference.portrait,
                health = 1
            };
            
            playerDataDictionary.Add(currentID, playerData);
            currentID++;
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

        var winnerTeam = LobbyPreferences.PlayerPreferences.Team.Blue;
        
        if (TryGetWinnerTeam(ref winnerTeam))
        {
            OnGameEnded.Raise(this, winnerTeam.ToString());
        }
    }

    public void OnPlayerPickedHealth(object sender, object data)
    {
        int playerID = (int)data;
        
        PlayerData tempData = playerDataDictionary[playerID];
        tempData.health++;
        playerDataDictionary[playerID] = tempData;
        
        PlayerInformationUI.Instance.UpdatePanels();
    }

    private bool TryGetWinnerTeam(ref LobbyPreferences.PlayerPreferences.Team winnerTeam)
    {
        var alivePlayersDataList = new List<PlayerData>();
        
        foreach (var playerData in playerDataDictionary.Values)
        {
            if (playerData.health > 0)
            {
                alivePlayersDataList.Add(playerData);
            }
        }

        var aliveTeam = alivePlayersDataList[0].team;
        
        foreach (var alivePlayerData in alivePlayersDataList)
        {
            if (alivePlayerData.team != aliveTeam)
            {
                return false;
            }
        }

        winnerTeam = aliveTeam;
        return true;
        
    }

    private IEnumerator WaitToSpawn(int deadPlayerID)
    {
        yield return Helpers.GetWait(1);

        PlayerData playerData = playerDataDictionary[deadPlayerID];
        SpawnPlayerWithID(playerData);
    }
    
    private void SpawnPlayerWithID(PlayerData playerData)
    {
        GameObject selectedPrefab = playerData.prefab;
        Transform selectedSpawnPositionTransform = spawnPositionTransformArray[playerData.playerID];
        
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
}