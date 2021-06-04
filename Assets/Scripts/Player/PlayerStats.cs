using System;
using Data.PlayerData;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public static event Action OnDie;
        public event Action<int> OnTakingDamage;
        public event Action<int> OnArmorChanged;

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Transform _spellTarget;
        private int _health;
        private int _armor;

        public PlayerData PlayerData => _playerData;
        public Transform SpellTarget => _spellTarget;

        private void Start()
        {
            _health = _playerData.Health;
            _armor = _playerData.Armor;
        }

        public void IncreaseArmor(int armor)
        {
            _armor += armor;
            OnArmorChanged?.Invoke(_armor);
        }

        public void TakeDamage(int damage)
        {
            if (_armor > 0)
            {
                _armor -= damage;
                OnArmorChanged?.Invoke(_armor);

                if (_armor >= 0) return;
                _health += _armor;
            }
            else _health -= damage;

            OnTakingDamage?.Invoke(_health);

            if (_health <= 0)
                OnDie?.Invoke();
        }
    }
}