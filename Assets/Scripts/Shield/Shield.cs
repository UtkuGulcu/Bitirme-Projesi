using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Player player;
    private ShieldAnimation shieldAnimation;
    private float duration;
    private float timer;
    private bool isShieldActive;

    private void Awake()
    {
        shieldAnimation = GetComponent<ShieldAnimation>();
        player = transform.parent.GetComponent<Player>();
    }

    private void Update()
    {
        if (!isShieldActive)
        {
            return;
        }
        
        timer += Time.deltaTime;

        if (timer >= duration - 3)
        {
            shieldAnimation.PlayDisableAnimation();
        }
    }

    public void Initialize(float duration)
    {
        isShieldActive = true;
        timer = 0f;
        this.duration = duration;
        gameObject.SetActive(true);
    }
    
    
    public void DisableShield()
    {
        isShieldActive = false;
        gameObject.SetActive(false);
        player.DisableShielded();
    }
}
