using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private WeaponAnimation weaponAnimation;

    private Player player;
    private float nextFireTime;
    private int ammo;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Player player)
    {
        this.player = player;
        transform.localPosition = weaponData.idleLocalPosition;
        ammo = weaponData.hasUnlimitedAmmo ? int.MaxValue : weaponData.maxAmmo;
        rb.simulated = false;
    }
    
    public void Fire(PlayerController.Direction direction, LobbyPreferences.PlayerPreferences.Team team)
    {
        GameObject spawnedObject = Instantiate(weaponData.bulletPrefab, barrelTransform.position, Quaternion.identity);
        Bullet bullet = spawnedObject.GetComponent<Bullet>();
        bullet.Setup(direction, weaponData, team);

        if (!weaponData.hasUnlimitedAmmo)
        {
            ammo--;
        }

        nextFireTime = Time.time + weaponData.fireRate;
        
        weaponAnimation.PlayFireAnimation();

        if (ammo <= 0)
        {
            ThrowEmptyWeapon(direction);
        }
        
    }

    public bool CanFire()
    {
        bool isReadyToFire = Time.time >= nextFireTime;

        return isReadyToFire && ammo >= 0;
    }

    private void ThrowEmptyWeapon(PlayerController.Direction direction)
    {
        transform.SetParent(null);
        rb.simulated = true;
        weaponAnimation.DisableAnimator();

        Vector2 forceDirection = (direction == PlayerController.Direction.Right) ? Vector2.right : Vector2.left;
        forceDirection += new Vector2(0, forceDirection.x / 2);

        float forceMagnitude = 100f;
        float torqueMagnitude = 10f;
        
        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        rb.AddTorque(torqueMagnitude, ForceMode2D.Impulse);

        StartCoroutine(player.WaitToEquipRevolver());
    }

    public bool HasUnlimitedAmmo()
    {
        return weaponData.hasUnlimitedAmmo;
    }

    public int GetRemainingAmmo()
    {
        return ammo;
    }

    public float GetKickForce(PlayerController.Direction direction)
    {
        float kickForce = weaponData.kickForce;

        if (direction == PlayerController.Direction.Right)
        {
            kickForce *= -1;
        }

        return kickForce;
    }
}
