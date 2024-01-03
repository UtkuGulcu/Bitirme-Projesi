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
            Red,
            Green,
            Blue,
            Yellow
        }

        public InputDevice inputDevice;
        public Team team;
        public GameObject playerPrefab;
        public string playerName;           //Maybe?

        public void SwitchToNextTeam()
        {
            Team[] values = (Team[])Enum.GetValues(typeof(Team));
            int currentIndex = Array.IndexOf(values, team);
            
            Team nextTeam = currentIndex < values.Length - 1 ? values[currentIndex + 1] : values[0];
            team = nextTeam;
        }

        public void ChangeSkin(GameObject newPrefab)
        {
            playerPrefab = newPrefab;
        }
    }

    private static List<PlayerPreferences> playerPreferencesList = new();

    public static List<PlayerPreferences> GetPlayerPreferencesList()
    {
        return playerPreferencesList;
    }

    public static bool TryToAddDevice(InputDevice inputDevice, GameObject defaultSkinPrefab)
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
            playerPrefab = defaultSkinPrefab
        };

        playerPreferencesList.Add(playerPreferences);
        
        return true;
    }

    private static bool IsDeviceRegistered(InputDevice inputDevice)
    {
        foreach (var playerPreference in playerPreferencesList)
        {
            if (playerPreference.inputDevice == inputDevice)
            {
                return true;
            }
        }
        
        return false;
    }

    public static void ChangeTeamOfPlayer(InputDevice inputDevice)
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDevice == inputDevice)
            {
                playerPreferences.SwitchToNextTeam();
            }
        }
    }

    public static void ChangeSkinOfPlayer(InputDevice inputDevice, GameObject newPrefab)
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDevice == inputDevice)
            {
                playerPreferences.ChangeSkin(newPrefab);
            }
        }
    }

    public static GameObject GetPrefabOfPlayer(InputDevice inputDevice)
    {
        foreach (var playerPreferences in playerPreferencesList)
        {
            if (playerPreferences.inputDevice == inputDevice)
            {
                return playerPreferences.playerPrefab;
            }
        }

        return null;
    }
}
