using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("References")]
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;

    [Header("Values")]
    public string weaponName;
    public Vector3 idleLocalPosition;
    public float bulletSpeed;
    public float impactForce;
    public float fireRate;
    public float kickForce;
    public bool hasUnlimitedAmmo;
    public int maxAmmo;
}
