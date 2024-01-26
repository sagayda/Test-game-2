using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Model.InGameScripts.Interfaces;

namespace Resources
{
    public class StatsController : MonoBehaviour
    {
        public Slider healthBar;
        public TMP_Text healthText;

        public Slider staminaBar;
        public TMP_Text staminaText;

        public Slider hungerBar;
        public TMP_Text hungerText;

        public Slider thirstBar;
        public TMP_Text thirstText;


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
            healthBar.value = _playerInfo.Health;
            healthText.text = $"{_playerInfo.Health}/{_playerInfo.MaxHealth}";

            staminaBar.maxValue = _playerInfo.MaxStamina;
            staminaBar.value = _playerInfo.Stamina;
            staminaText.text = $"{_playerInfo.Stamina}/{_playerInfo.MaxStamina}";

            hungerBar.maxValue = _playerInfo.MaxHunger;
            hungerBar.value = _playerInfo.Hunger;
            hungerText.text = $"{_playerInfo.Hunger}/{_playerInfo.MaxHunger}";

            thirstBar.maxValue = _playerInfo.MaxThirst;
            thirstBar.value = _playerInfo.Thirst;
            thirstText.text = $"{_playerInfo.Thirst}/{_playerInfo.MaxThirst}";
        }

        public void UpdateStats()
        {
            if (_playerInfo == null)
                return;

            healthBar.value = _playerInfo.Health;
            healthText.text = $"{_playerInfo.Health}/{_playerInfo.MaxHealth}";

            staminaBar.value = _playerInfo.Stamina;
            staminaText.text = $"{_playerInfo.Stamina}/{_playerInfo.MaxStamina}";

            hungerBar.value = _playerInfo.Hunger;
            hungerText.text = $"{_playerInfo.Hunger}/{_playerInfo.MaxHunger}";

            thirstBar.value = _playerInfo.Thirst;
            thirstText.text = $"{_playerInfo.Thirst}/{_playerInfo.MaxThirst}";
        }
    }
}
