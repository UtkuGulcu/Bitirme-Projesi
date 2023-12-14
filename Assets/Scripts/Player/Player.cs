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
    private float pushForce;
    private float pushTimer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PickupWeapon(defaultWeapon);
    }

    private void Update()
    {
        CheckDeathArea();
    }

    private void FixedUpdate()
    {
        if (pushTimer < 0.7f)
        {
            pushTimer += Time.fixedDeltaTime;
            float newForce = Mathf.Lerp(pushForce, 0, pushTimer);
            rb.AddForce(Vector2.right * (newForce * Time.fixedDeltaTime), ForceMode2D.Force);
        }
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
        if (!weapon.CanFire()) return;
        
        weapon.Fire(playerController.GetDirection());
        
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

    private void PickupWeapon(WeaponSO weaponSO)
    {
        GameObject spawnedObject = Instantiate(weaponSO.weaponPrefab, transform);
        weapon = spawnedObject.GetComponent<Weapon>();
        weapon.Setup();
        
        PlayerManager.Instance.UpdateWeaponNameWithID(playerID, weapon.GetWeaponName());
    }

    public void GetHit(float force)
    {
        pushForce = force;
        pushTimer = 0;
        rb.AddForce(Vector2.right * force * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void SetPlayerID(int ID)
    {
        playerID = ID;
    }

    private void Die()
    {
        OnPlayerDied.Raise(this, playerID);
        Destroy(gameObject);
    }
}
