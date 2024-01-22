using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePickupVisual : MonoBehaviour
{
    [SerializeField] private Sprite onAirSprite;
    [SerializeField] private Sprite onGroundSprite;
    
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        EnableAirVisual();
    }

    private void EnableAirVisual()
    {
        spriteRenderer.sprite = onAirSprite;
    }
    
    public void EnableGroundVisual()
    {
        spriteRenderer.sprite = onGroundSprite;
    }
}
