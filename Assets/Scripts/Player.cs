using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    
    public void TryToShoot()
    {
        playerAnimation.PlayShootAnimation();
    }
}
