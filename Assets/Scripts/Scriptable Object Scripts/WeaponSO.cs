using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("References")]
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;

    [Header("Values")]
    public string weaponName;
    public Vector2 localPosition;
    public float bulletSpeed;
    public float impactForce;
    public float fireRate;
}
