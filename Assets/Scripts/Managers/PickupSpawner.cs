using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform[] spawnLocationTransformArray;

    private float timer;
    private float timerMax = 5f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timerMax)
        {
            timer = 0f;
            timerMax = 20f;
            StartCoroutine(SpawnPickups());
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int randomIndex = Random.Range(0, spawnLocationTransformArray.Length);
            Vector3 spawnLocation = spawnLocationTransformArray[randomIndex].position;
            Instantiate(pickupPrefab, spawnLocation, Quaternion.identity);
        }
    }

    private IEnumerator SpawnPickups()
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
}
