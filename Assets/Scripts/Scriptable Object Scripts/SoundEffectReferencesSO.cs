using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Effect References")]
public class SoundEffectReferencesSO : ScriptableObject
{
    public AudioClip weaponPickup;
    public AudioClip winScreen;
    public AudioClip speedPickup;
    public AudioClip shieldPickup;
    public AudioClip healthPickup;
    public AudioClip revolverShoot;
    public AudioClip fullAutoShoot;
    public AudioClip sniperShoot;
    public AudioClip uziShoot;
}