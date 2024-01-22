using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInformationPanelSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text healthCountText;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text bulletCountText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image portraitImage;
    
    public void SetupPanel(PlayerManager.PlayerData playerData)
    {
        UpdateText(playerData);

        backgroundImage.color = playerData.teamColor;
        portraitImage.sprite = playerData.portraitSprite;
    }
    
    public void UpdateText(PlayerManager.PlayerData playerData)
    {
        playerNameText.text = $"Player {playerData.playerID}";
        healthCountText.text = playerData.health.ToString();
    }

    public void UpdateWeaponInfo(string newWeaponName, int ammo)
    {
        weaponNameText.text = newWeaponName;
        bulletCountText.text = ammo.ToString();
    }

    public void UpdateBulletCount(int remainingAmmo)
    {
        bulletCountText.text = remainingAmmo.ToString();
    }
}
