using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    private Bullet bullet;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
        {
            return;
        }

        if (player.IsTeamMate(bullet.GetOwnerTeam()))
        {
            return;
        }
        
        float force = bullet.GetImpactForce();

        if (bullet.GetDirection() == PlayerController.Direction.Left)
        {
            force *= -1;
        }
            
        player.GetHit(force);
        bullet.DestroySelf();
    }
}
