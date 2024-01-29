using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickupTrigger : MonoBehaviour
{
    [SerializeField] private GameEventSO OnShieldPickupPicked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnShieldPickupPicked.Raise();
            player.EnableShield(7);
            Destroy(gameObject);
        }
    }
}
