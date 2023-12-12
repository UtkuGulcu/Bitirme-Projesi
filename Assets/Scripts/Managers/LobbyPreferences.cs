using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class LobbyPreferences
{
    private static List<InputDevice> activeDeviceList = new List<InputDevice>();

    public static List<InputDevice> GetActiveDevices()
    {
        return activeDeviceList;
    }

    public static void AddDevice(InputDevice inputDevice)
    {
        activeDeviceList.Add(inputDevice);
    }
}
