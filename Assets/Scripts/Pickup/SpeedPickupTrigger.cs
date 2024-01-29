using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickupTrigger : MonoBehaviour
{
    [SerializeField] private GameEventSO OnSpeedPickupPicked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnSpeedPickupPicked.Raise();
            player.IncreaseMovementSpeed();
            Destroy(gameObject);
        }
    }
}
