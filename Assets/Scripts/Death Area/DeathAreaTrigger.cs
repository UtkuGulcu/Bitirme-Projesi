using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }
}
