using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class LobbyPreferences
{
    private static List<InputDevice> activeDevices = new List<InputDevice>();

    public static List<InputDevice> GetActiveDevices()
    {
        return activeDevices;
    }

    public static void AddDevice(InputDevice inputDevice)
    {
        activeDevices.Add(inputDevice);
    }
}
