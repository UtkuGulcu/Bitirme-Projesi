using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupTrigger : MonoBehaviour
{
    [SerializeField] private WeaponSO[] weaponArray;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.PickupWeapon(GetRandomWeapon());
            Destroy(gameObject);
        }
    }

    private WeaponSO GetRandomWeapon()
    {
        int randomIndex = Random.Range(0, weaponArray.Length - 1);
        return weaponArray[randomIndex];
    }
}