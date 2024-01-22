using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/Team Colors")]
public class TeamColorsSO : ScriptableObject
{
    [Serializable]
    public class TeamColor
    {
        public string teamName;
        public Color color;
    }

    public TeamColor[] teamColorArray;

    public Color GetNextColor(Color currentColor)
    {
        int currentIndex = 0;
        
        foreach (var teamColor in teamColorArray)
        {
            if (teamColor.color == currentColor)
            {
                currentIndex = Array.IndexOf(teamColorArray, teamColor);
            }
        }

        currentIndex++;
        currentIndex %= teamColorArray.Length;
        return teamColorArray[currentIndex].color;
    }

    public Color GetDefaultColor()
    {
        return teamColorArray[0].color;
    }

    public Color GetColorWithName(string teamName)
    {
        foreach (var teamColor in teamColorArray)
        {
            if (teamColor.teamName == teamName)
            {
                return teamColor.color;
            }
        }

        return GetDefaultColor();
    }
}
