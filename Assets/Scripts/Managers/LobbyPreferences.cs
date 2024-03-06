using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class LobbyPreferences
{
    public class PlayerPreferences
    {
        public class InputDeviceData
        {
            public InputDevice inputDevice;
            public bool isSecondKeyboard; // Must be set if input device is keyboard
        }
        
        public enum Team
        {
            Blue,
            Red,
            Green,
            Yellow
        }

        public InputDeviceData inputDeviceData;
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

    public static void AddDevice(InputDevice inputDevice, GameObject defaultSkinPrefab, Sprite defaultPortraitSprite, bool isSecondKeyboard = false)
    {
        var inputDeviceData = new PlayerPreferences.InputDeviceData
        {
            inputDevice = inputDevice,
            isSecondKeyboard = isSecondKeyboard
        };


        var playerPreferences = new PlayerPreferences
        {
            team = PlayerPreferences.Team.Blue,
            inputDeviceData = inputDeviceData,
            playerName = "Player",
            playerPrefab = defaultSkinPrefab,
            portrait = defaultPortraitSprite
        };

        playerPreferencesList.Add(playerPreferences);
    }

    public static bool IsDeviceRegistered(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        return FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard) != null;
    }

    public static void ChangeTeamOfPlayer(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        playerPreferences.SwitchToNextTeam();
    }

    public static void ChangeSkinOfPlayer(InputDevice inputDevice, GameObject newPrefab, Sprite portraitSprite, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        playerPreferences.ChangeSkin(newPrefab, portraitSprite);
    }

    public static GameObject GetPrefabOfPlayer(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        return playerPreferences.playerPrefab;
    }
    
    public static Sprite GetPortraitOfPlayer(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        return playerPreferences.portrait;
    }

    public static void SetPlayerReady(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        playerPreferences.isReady = true;
        
        StartGameIfReady();
    }
    
    public static IEnumerator SetPlayerNotReady(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        yield return new WaitForEndOfFrame();
        
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        playerPreferences.isReady = false;
    }

    public static bool IsPlayerReady(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
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

    private static PlayerPreferences FindPlayerPreferencesWithInputDevice(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDeviceData.inputDevice == inputDevice && playerPreferences.inputDeviceData.isSecondKeyboard == isSecondKeyboard)
            {
                return playerPreferences;
            }
        }

        return null;
    }

    public static void DeletePlayerPreferences(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var playerPreferences = FindPlayerPreferencesWithInputDevice(inputDevice, isSecondKeyboard);
        playerPreferencesList.Remove(playerPreferences);
    }

    public static int GetPlayerCount()
    {
        return playerPreferencesList.Count;
    }

    public static bool IsAnyKeyboardRegistered()
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDeviceData.inputDevice is Keyboard)
            {
                return true;
            }
        }

        return false;
    }
}
