using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPanelAnimation : MonoBehaviour
{
    private static readonly int TriggerID = Animator.StringToHash("Open");
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayOpenAnimation()
    {
        animator.SetTrigger(TriggerID);
    }
}
