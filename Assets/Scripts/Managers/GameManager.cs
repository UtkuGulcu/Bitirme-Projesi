using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum Level
    {
        Arena,
        Space
    }
    
    [Header("References")]
    [SerializeField] private GameEventSO OnArenaMusicPlayed;
    [SerializeField] private GameEventSO OnSpaceMusicPlayed;

    [Header("Values")]
    [SerializeField] private Level level;

    private void Start()
    {
        if (level == Level.Arena)
        {
            OnArenaMusicPlayed.Raise();
        }
        else
        {
            OnSpaceMusicPlayed.Raise();
        }
    }
}
