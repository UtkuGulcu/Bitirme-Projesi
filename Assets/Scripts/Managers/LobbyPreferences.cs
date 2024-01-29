using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class LobbyPreferences
{
    public class PlayerPreferences
    {
        public enum Team
        {
            Blue,
            Red,
            Green,
            Yellow
        }

        public InputDevice inputDevice;
        public Team team;
        public GameObject playerPrefab;
        public Sprite portrait;
        public string playerName;           //Maybe?
        public bool isReady;

        public void SwitchToNextTeam()
        {
            Team[] values = (Team[])Enum.GetValues(typeof(Team));
            int currentIndex = Array.IndexOf(values, team);
            
            Team nextTeam = currentIndex < values.Length - 1 ? values[currentIndex + 1] : values[0];
            team = nextTeam;
        }

        public void ChangeSkin(GameObject newPrefab, Sprite portraitSprite)
        {
            playerPrefab = newPrefab;
            portrait = portraitSprite;
        }
    }

    private static List<PlayerPreferences> playerPreferencesList = new();

    public static List<PlayerPreferences> GetPlayerPreferencesList()
    {
        return playerPreferencesList;
    }

    public static bool TryToAddDevice(InputDevice inputDevice, GameObject defaultSkinPrefab, Sprite defaultPortraitSprite)
    {
        if (IsDeviceRegistered(inputDevice) || inputDevice == null)
        {
            return false;
        }

        var playerPreferences = new PlayerPreferences
        {
            team = PlayerPreferences.Team.Blue,
            inputDevice = inputDevice,
            playerName = "Player",
            playerPrefab = defaultSkinPrefab,
            portrait = defaultPortraitSprite
        };

        playerPreferencesList.Add(playerPreferences);
        
        return true;
    }

    private static bool IsDeviceRegistered(InputDevice inputDevice)
    {
        return FindPlayerPreferencesWithInputDevice(inputDevice) != null;
    }

    public static void ChangeTeamOfPlayer(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        playerPreferences.SwitchToNextTeam();
    }

    public static void ChangeSkinOfPlayer(InputDevice inputDevice, GameObject newPrefab, Sprite portraitSprite)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        playerPreferences.ChangeSkin(newPrefab, portraitSprite);
    }

    public static GameObject GetPrefabOfPlayer(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        return playerPreferences.playerPrefab;
    }
    
    public static Sprite GetPortraitOfPlayer(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        return playerPreferences.portrait;
    }

    public static void SetPlayerReady(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        playerPreferences.isReady = true;
        
        StartGameIfReady();
    }
    
    public static IEnumerator SetPlayerNotReady(InputDevice inputDevice)
    {
        yield return new WaitForEndOfFrame();
        
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        playerPreferences.isReady = false;
    }

    public static bool IsPlayerReady(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        return playerPreferences.isReady;
    }

    private static void StartGameIfReady()
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (!playerPreferences.isReady)
            {
                return;
            }
        }

        if (LobbyUI.Instance.GetSelectedLevel() == LobbyUI.Level.Arena)
        {
            SceneLoader.LoadArenaLevel();
        }
        else
        {
            SceneLoader.LoadSpaceLevel();
        }
    }

    public static void ClearMemory()
    {
        playerPreferencesList.Clear();
    }

    private static PlayerPreferences FindPlayerPreferencesWithInputDevice(InputDevice inputDevice)
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDevice == inputDevice)
            {
                return playerPreferences;
            }
        }

        return null;
    }

    public static void DeletePlayerPreferences(InputDevice inputDevice)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice);
        playerPreferencesList.Remove(playerPreferences);
    }

    public static int GetPlayerCount()
    {
        return playerPreferencesList.Count;
    }
}
