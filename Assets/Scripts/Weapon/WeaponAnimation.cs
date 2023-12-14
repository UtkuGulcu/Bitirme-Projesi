using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private static readonly int fireTriggerID = Animator.StringToHash("fire");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayFireAnimation()
    {
        animator.SetTrigger(fireTriggerID);
    }
}
