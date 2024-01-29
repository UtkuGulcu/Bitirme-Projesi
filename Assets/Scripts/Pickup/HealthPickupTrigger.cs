using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupTrigger : MonoBehaviour
{
    [SerializeField] private GameEventSO OnHealthPickupPicked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnHealthPickupPicked.Raise();
            player.IncreaseHealth();
            Destroy(gameObject);
        }
    }
}
