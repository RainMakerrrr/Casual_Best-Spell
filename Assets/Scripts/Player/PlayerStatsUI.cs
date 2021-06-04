using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TextMeshProUGUI _armorText;

        private PlayerStats _playerStats;

        private void OnEnable()
        {
            _playerStats = GetComponent<PlayerStats>();
            _healthBar.maxValue = _playerStats.PlayerData.Health;
            _healthBar.value = _playerStats.PlayerData.Health;

            _playerStats.OnTakingDamage += UpdateStatsBar;
            _playerStats.OnArmorChanged += UpdateArmorText;
        }

        private void UpdateStatsBar(int health)
        {
            _healthBar.DOValue(health, 0.5f);
        }

        private void UpdateArmorText(int armor)
        {
            _armorText.text = armor.ToString();
        }

        private void OnDisable()
        {
            _playerStats.OnTakingDamage -= UpdateStatsBar;
            _playerStats.OnArmorChanged -= UpdateArmorText;
        }
    }
}