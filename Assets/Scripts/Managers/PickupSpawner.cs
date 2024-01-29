using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform[] spawnLocationTransformArray;
    [SerializeField] private GameObject[] upgradePickupPrefabArray;

    private float weaponPickupTimer;
    private float weaponPickupTimerMax = 5f;
    private float upgradePickupTimer;
    private float upgradePickupTimerMax = 5f;

    private void Update()
    {
        weaponPickupTimer += Time.deltaTime;
        upgradePickupTimer += Time.deltaTime;

        if (weaponPickupTimer >= weaponPickupTimerMax)
        {
            weaponPickupTimer = 0f;
            weaponPickupTimerMax = 20f;
            StartCoroutine(SpawnWeaponPickups());
        }
        
        if (upgradePickupTimer >= upgradePickupTimerMax)
        {
            upgradePickupTimer = 0f;
            upgradePickupTimerMax = 15f;
            StartCoroutine(SpawnUpgradePickups());
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int randomIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomIndex].position;
            Instantiate(pickupPrefab, spawnLocation, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int randomSpawnIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomSpawnIndex].position;
            
            GameObject randomPrefab = upgradePickupPrefabArray[0];
            
            Instantiate(randomPrefab, spawnLocation, Quaternion.identity);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int randomSpawnIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomSpawnIndex].position;
            
            GameObject randomPrefab = upgradePickupPrefabArray[1];
            
            Instantiate(randomPrefab, spawnLocation, Quaternion.identity);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int randomSpawnIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomSpawnIndex].position;
            
            GameObject randomPrefab = upgradePickupPrefabArray[2];
            
            Instantiate(randomPrefab, spawnLocation, Quaternion.identity);
        }
    }

    private IEnumerator SpawnWeaponPickups()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        
        for (int i = 0; i < LobbyPreferences.GetPlayerCount() - 1; i++)
        {
            int randomIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomIndex].position;
            Instantiate(pickupPrefab, spawnLocation, Quaternion.identity);

            yield return waitForEndOfFrame;
        }
    }
    
    private IEnumerator SpawnUpgradePickups()
    {
        var waitForEndOfFrame = new WaitForEndOfFrame();
        
        for (int i = 0; i < LobbyPreferences.GetPlayerCount() - 1; i++)
        {
            int randomSpawnIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomSpawnIndex].position;

            int randomPrefabIndex = Random.Range(0, upgradePickupPrefabArray.Length);
            GameObject randomPrefab = upgradePickupPrefabArray[randomPrefabIndex];
            
            Instantiate(randomPrefab, spawnLocation, Quaternion.identity);

            yield return waitForEndOfFrame;
        }
    }
}
