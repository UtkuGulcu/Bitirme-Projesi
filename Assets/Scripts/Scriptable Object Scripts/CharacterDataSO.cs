using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Serializable]
    public class Character
    {
        public GameObject prefab;
        public Sprite lobbySprite;
    }
    
    public Character[] characterArray;

    public GameObject GetNextPrefab(GameObject currentGameObject)
    {
        int index = 0;
        
        foreach (var character in characterArray)
        {
            if (currentGameObject == character.prefab)
            {
                index = Array.IndexOf(characterArray, character);
            }
        }

        index++;
        index %= characterArray.Length;

        return characterArray[index].prefab;
    }
    
    public GameObject GetPreviousPrefab(GameObject currentGameObject)
    {
        int index = 0;
        
        foreach (var character in characterArray)
        {
            if (currentGameObject == character.prefab)
            {
                index = Array.IndexOf(characterArray, character);
            }
        }

        index--;

        if (index < 0)
        {
            index = characterArray.Length - 1;
        }

        return characterArray[index].prefab;
    }

    public Sprite GetNextCharacterSprite(Sprite currentSprite)
    {
        int index = 0;
        
        foreach (var character in characterArray)
        {
            if (currentSprite == character.lobbySprite)
            {
                index = Array.IndexOf(characterArray, character);
            }
        }

        index++;
        index %= characterArray.Length;

        return characterArray[index].lobbySprite;
    }
    
    public Sprite GetPreviousCharacterSprite(Sprite currentSprite)
    {
        int index = 0;
        
        foreach (var character in characterArray)
        {
            if (currentSprite == character.lobbySprite)
            {
                index = Array.IndexOf(characterArray, character);
            }
        }

        index--;
        
        if (index < 0)
        {
            index = characterArray.Length - 1;
        }

        return characterArray[index].lobbySprite;
    }

    public Sprite GetDefaultCharacterSprite()
    {
        return characterArray[0].lobbySprite;
    }

    public GameObject GetDefaultSkinPrefab()
    {
        return characterArray[0].prefab;
    }
}
