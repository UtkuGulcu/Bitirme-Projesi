using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private MusicReferencesSO musicReferences;
    [SerializeField] private SoundEffectReferencesSO soundEffectReferences;

    [SerializeField] private AudioSource mainMenuAudioSource;

    private bool isSoundMuted;
    private bool isMusicMuted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuMusic()
    {
        if (isMusicMuted)
        {
            return;
        }
        
        if (!mainMenuAudioSource.isPlaying)
        {
            mainMenuAudioSource.Play();
        }
    }

    public void PlaySpaceMusic()
    {
        if (isMusicMuted)
        {
            return;
        }
        
        mainMenuAudioSource.Stop();
        PlaySound(musicReferences.spaceMusic, 0.5f);
    }

    public void PlayArenaMusic()
    {
        if (isMusicMuted)
        {
            return;
        }
        
        mainMenuAudioSource.Stop();
        PlaySound(musicReferences.arenaMusic, 0.15f);
    }

    public void PlayWeaponPickupPickedSoundEffect()
    {
        PlaySound(soundEffectReferences.weaponPickup, 0.3f);
    }

    public void PlayWinScreenSoundEffect()
    {
        PlaySound(soundEffectReferences.winScreen, 0.3f);
    }
    
    public void PlaySpeedPickupSoundEffect()
    {
        PlaySound(soundEffectReferences.speedPickup, 0.3f);
    }
    
    public void PlayShieldPickupSoundEffect()
    {
        PlaySound(soundEffectReferences.shieldPickup, 0.3f);
    }
    
    public void PlayHealthPickupSoundEffect()
    {
        PlaySound(soundEffectReferences.healthPickup, 0.3f);
    }

    public void PlayWeaponShootSoundEffect(object sender, object data)
    {
        string weaponName = data as string;

        AudioClip shootClip = null;

        float volume = 0.2f;
        
        if (weaponName == "Revolver")
        {
            shootClip = soundEffectReferences.revolverShoot;
        }
        else if (weaponName == "Automatic Rifle")
        {
            shootClip = soundEffectReferences.fullAutoShoot;
        }
        else if (weaponName == "Sniper")
        {
            shootClip = soundEffectReferences.sniperShoot;
        }
        else if (weaponName == "Uzi")
        {
            shootClip = soundEffectReferences.uziShoot;
            volume = 1f;
        }
        
        PlaySound(shootClip, volume);
    }
    
    private void PlaySound(AudioClip audioClip, float volume)
    {
        if (isSoundMuted)
        {
            return;
        }
        
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
    }

    public void MuteSound(bool isMuted)
    {
        isSoundMuted = isMuted;
    }
    
    public void MuteMusic(bool isMuted)
    {
        if (isMuted)
        {
            mainMenuAudioSource.volume = 0;
        }

        mainMenuAudioSource.volume = isMuted ? 0 : 0.1f;
        
        isMusicMuted = isMuted;
    }
}
