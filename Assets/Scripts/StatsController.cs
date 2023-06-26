using Assets.Scripts.InGameScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;
    public Slider hungerBar;
    public Slider thirstBar;

    private IPlayerInfo _playerInfo;

    public void SetStats(IPlayerInfo playerInfo)
    {
        this._playerInfo = playerInfo;
        SetStats();
    }

    private void SetStats()
    {
        if (_playerInfo == null)
            return;

        healthBar.maxValue = _playerInfo.MaxHealth;
        staminaBar.maxValue = _playerInfo.MaxStamina;
        hungerBar.maxValue = _playerInfo.MaxHunger;
        thirstBar.maxValue = _playerInfo.MaxThirst;

        healthBar.value = _playerInfo.Health;
        staminaBar.value = _playerInfo.Stamina;
        hungerBar.value = _playerInfo.Hunger;
        thirstBar.value = _playerInfo.Thirst;
    }

    public void UpdateStats()
    {
        if (_playerInfo == null) 
            return;

        healthBar.value = _playerInfo.Health;
        staminaBar.value = _playerInfo.Stamina;
        hungerBar.value = _playerInfo.Hunger;
        thirstBar.value = _playerInfo.Thirst;
    }
}
