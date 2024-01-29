using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponPickupTrigger : MonoBehaviour
{
    [SerializeField] private WeaponSO[] weaponArray;
    [SerializeField] private GameEventSO OnWeaponPickupPicked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.PickupWeapon(GetRandomWeapon());
            OnWeaponPickupPicked.Raise();
            Destroy(gameObject);
        }
    }

    private WeaponSO GetRandomWeapon()
    {
        int randomIndex = Random.Range(0, weaponArray.Length);
        return weaponArray[randomIndex];
    }
}