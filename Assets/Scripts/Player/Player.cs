using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEventSO OnPlayerDied;
    
    [Header("References")]
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private GroundedCheck groundedCheck;
    [SerializeField] private WeaponSO defaultWeapon;

    private PlayerController playerController;
    private Weapon weapon;
    private Rigidbody2D rb;
    private bool canDoubleJump = true;
    private int playerID;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PickupWeapon(defaultWeapon);
    }

    public void TryToShoot()
    {
        if (weapon.CanFire())
        {
            playerAnimation.PlayShootAnimation();
            weapon.Fire(playerController.GetDirection());
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

    private void PickupWeapon(WeaponSO weaponSO)
    {
        GameObject spawnedObject = Instantiate(weaponSO.weaponPrefab, transform);
        weapon = spawnedObject.GetComponent<Weapon>();
        weapon.Setup();
        
        PlayerManager.Instance.UpdateWeaponNameWithID(playerID, weapon.GetWeaponName());
    }

    public void GetHit(float force)
    {
        rb.AddForce(Vector2.right * force * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void SetPlayerID(int ID)
    {
        playerID = ID;
    }

    public void Die()
    {
        OnPlayerDied.Raise(this, playerID);
        Destroy(gameObject);
    }
}
