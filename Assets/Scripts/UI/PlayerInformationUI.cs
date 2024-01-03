using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInformationUI : MonoBehaviour
{
    public static PlayerInformationUI Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private GameObject playerInformationPanelTemplate;
    [SerializeField] private Transform playerInformationPanelParentTransform;

    private List<PlayerInformationPanelSingleUI> panelList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are multiple PlayerInformationUI");
            Destroy(this);
        }
    }

    private void Start()
    {
        panelList = new List<PlayerInformationPanelSingleUI>();
        SpawnPlayerInformationPanels();
    }

    private void SpawnPlayerInformationPanels()
    {
        foreach (var playerData in PlayerManager.Instance.GetAllPlayerInformation())
        {
            GameObject spawnedObject = Instantiate(playerInformationPanelTemplate, playerInformationPanelParentTransform);
            spawnedObject.SetActive(true);

            PlayerInformationPanelSingleUI playerInformationPanelSingleUI = spawnedObject.GetComponent<PlayerInformationPanelSingleUI>();
            playerInformationPanelSingleUI.UpdateText(playerData);
            panelList.Add(playerInformationPanelSingleUI);
        }
    }

    public void UpdatePanels()
    {
        int count = 0;
        
        foreach (var playerData in PlayerManager.Instance.GetAllPlayerInformation())
        {
            panelList[count].UpdateText(playerData);
            count++;
        }
    }

    public void UpdateWeaponInfo(object sender, object data)
    {
        var args = data as GameEventArgs.OnWeaponPickedEventArgs;
        int index = args.playerID - 1;
        string weaponName = args.weaponName;
        int ammo = args.ammo;
        
        panelList[index].UpdateWeaponInfo(weaponName, ammo);
    }
    
    public void UpdateBulletCount(object sender, object data)
    {
        var args = data as GameEventArgs.OnShotFiredEventArgs;
        int index = args.playerID - 1;
        panelList[index].UpdateBulletCount(args.remainingAmmo);
    }
}
