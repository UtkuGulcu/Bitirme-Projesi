using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private WeaponSO weaponData;
    private Rigidbody2D rb;
    private PlayerController.Direction direction;
    private LobbyPreferences.PlayerPreferences.Team ownerTeam;
    private float timer;
    private const float TIMER_MAX = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= TIMER_MAX)
        {
            DestroySelf();
        }
    }

    public void Setup(PlayerController.Direction playerDirection, WeaponSO weaponData, LobbyPreferences.PlayerPreferences.Team team)
    {
        this.weaponData = weaponData;
        this.ownerTeam = team;
        
        direction = playerDirection;
        
        Vector2 force = transform.right * weaponData.bulletSpeed;
        
        if (direction == PlayerController.Direction.Left)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            force *= -1;
        }

        rb.velocity = force;
    }

    public PlayerController.Direction GetDirection()
    {
        return direction;
    }

    public float GetImpactForce()
    {
        return weaponData.impactForce;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public LobbyPreferences.PlayerPreferences.Team GetOwnerTeam()
    {
        return ownerTeam;
    }
}
