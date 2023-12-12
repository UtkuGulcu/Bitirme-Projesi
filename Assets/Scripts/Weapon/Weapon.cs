using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Transform barrelTransform;

    private float nextFireTime;

    public void Setup()
    {
        transform.localPosition = weaponData.localPosition;
    }
    
    public void Fire(PlayerController.Direction direction)
    {
        GameObject spawnedObject = Instantiate(weaponData.bulletPrefab, barrelTransform.position, Quaternion.identity);
        Bullet bullet = spawnedObject.GetComponent<Bullet>();
        bullet.Setup(direction);

        nextFireTime = Time.time + weaponData.fireRate;
    }

    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    public string GetWeaponName()
    {
        return weaponData.weaponName;
    }
}
