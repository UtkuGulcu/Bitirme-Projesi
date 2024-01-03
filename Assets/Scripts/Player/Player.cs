using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEventSO OnPlayerDied;
    [SerializeField] private GameEventSO OnWeaponPicked;
    [SerializeField] private GameEventSO OnShotFired;

    [Header("References")]
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Shield shield;
    [SerializeField] private GroundedCheck groundedCheck;
    [SerializeField] private WeaponSO defaultWeapon;

    private PlayerController playerController;
    private Weapon weapon;
    private LobbyPreferences.PlayerPreferences.Team team;
    private bool canDoubleJump = true;
    private int playerID;
    private bool isShielded;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        PickupWeapon(defaultWeapon);
        EnableShield(3);
    }

    private void Update()
    {
        CheckDeathArea();
    }

    private void CheckDeathArea()
    {
        if (transform.position.y <= -9f)
        {
            Die();
        }
    }

    public void TryToShoot()
    {
        if (weapon == null) return;
        if (!weapon.CanFire()) return;

        weapon.Fire(playerController.GetDirection(), team);

        RaiseShotFiredEvent();

        if (groundedCheck.IsGrounded())
        {
            playerAnimation.PlayShootAnimation();
        }
    }

    public void TryToJump()
    {
        if (groundedCheck.IsGrounded())
        {
            playerController.Jump();
            playerAnimation.PlayJumpAnimation();
        }
        else if (canDoubleJump)
        {
            playerController.Jump();
            playerAnimation.PlayJumpAnimation();
            canDoubleJump = false;
        }
    }

    public void TryToDropDown()
    {
        if (groundedCheck.IsGrounded())
        {
            playerController.DropDown();
        }
    }

    public void RefreshDoubleJump()
    {
        canDoubleJump = true;
    }

    public void PickupWeapon(WeaponSO weaponSO)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }
        
        GameObject spawnedObject = Instantiate(weaponSO.weaponPrefab, transform);
        weapon = spawnedObject.GetComponent<Weapon>();
        weapon.Setup(this);

        var onWeaponPickedEventArgs = GameEventArgs.GetOnWeaponPickedEventArgs(playerID, weaponSO.weaponName, weaponSO.maxAmmo);
        OnWeaponPicked.Raise(this, onWeaponPickedEventArgs);
    }
    
    public void GetHit(float force)
    {
        if (isShielded)
        {
            return;
        }

        playerController.GetPushed(force);
    }

    public void SetPlayerData(int ID, LobbyPreferences.PlayerPreferences.Team team)
    {
        playerID = ID;
        this.team = team;
    }

    private void Die()
    {
        OnPlayerDied.Raise(this, playerID);
        Destroy(gameObject);
    }

    private void EnableShield(float duration)
    {
        isShielded = true;
        shield.Initialize(duration);
    }

    public void DisableShielded()
    {
        isShielded = false;
    }

    public IEnumerator WaitToEquipRevolver()
    {
        yield return new WaitForEndOfFrame();
        
        weapon = null;
        
        yield return Helpers.GetWait(1f);
        
        PickupWeapon(defaultWeapon);
    }

    private void RaiseShotFiredEvent()
    {
        if (weapon.HasUnlimitedAmmo()) return;
        
        var onShotFiredEventArgs = GameEventArgs.GetOnShotFiredEventArgs(playerID, weapon.GetRemainingAmmo());
        OnShotFired.Raise(this, onShotFiredEventArgs);
    }

    public bool IsTeamMate(LobbyPreferences.PlayerPreferences.Team comparedTeam)
    {
        return team == comparedTeam;
    }
}