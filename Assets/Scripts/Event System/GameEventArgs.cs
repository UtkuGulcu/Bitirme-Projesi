using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameEventArgs
{
    public class OnWeaponPickedEventArgs
    {
        public int playerID;
        public string weaponName;
        public int ammo;
    }
    
    public class OnShotFiredEventArgs
    {
        public int playerID;
        public int remainingAmmo;
    }

    public class OnPlayerChangedSkinEventArgs
    {
        public bool hasSwitchedToNextSkin;
        public InputDevice inputDevice;
        public bool isSecondKeyboard;
    }

    public class InputDeviceEventArgs
    {
        public InputDevice inputDevice;
        public bool isSecondKeyboard;
    }

    public static OnWeaponPickedEventArgs GetOnWeaponPickedEventArgs(int playerID, string weaponName, int ammo)
    {
        var onWeaponPickedEventArgs = new OnWeaponPickedEventArgs
        {
            playerID = playerID,
            weaponName = weaponName,
            ammo = ammo
        };

        return onWeaponPickedEventArgs;
    }
    
    public static OnShotFiredEventArgs GetOnShotFiredEventArgs(int playerID, int remainingAmmo)
    {
        var onShotFiredEventArgs = new OnShotFiredEventArgs
        {
            playerID = playerID,
            remainingAmmo = remainingAmmo
        };

        return onShotFiredEventArgs;
    }

    public static OnPlayerChangedSkinEventArgs GetOnPlayerChangedSkinEventArgs(bool hasSwitchedToNextSkin, InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var onPlayerChangedSkinEventArgs = new OnPlayerChangedSkinEventArgs
        {
            hasSwitchedToNextSkin = hasSwitchedToNextSkin,
            inputDevice = inputDevice,
            isSecondKeyboard = isSecondKeyboard
        };

        return onPlayerChangedSkinEventArgs;
    }

    public static InputDeviceEventArgs GetInputDeviceEventArgs(InputDevice inputDevice, bool isSecondKeyboard = false)
    {
        var inputDeviceEventArgs = new InputDeviceEventArgs
        {
            inputDevice = inputDevice,
            isSecondKeyboard = isSecondKeyboard
        };

        return inputDeviceEventArgs;
    }
}
