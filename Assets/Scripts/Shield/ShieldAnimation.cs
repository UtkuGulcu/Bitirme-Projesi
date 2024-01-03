using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAnimation : MonoBehaviour
{
    private static readonly int disableTriggerID = Animator.StringToHash("disable");
    
    private Animator animator;
    private Shield shield;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        shield = GetComponent<Shield>();
    }

    public void PlayDisableAnimation()
    {
        animator.SetTrigger(disableTriggerID);
    }

    // Animation Event: Gets Triggered when the disable animation ends
    public void OnAnimationEnded()
    {
        shield.DisableShield();
    }
}
