using System;
using Data;
using Data.Enemies;
using GameState;
using Player;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        public event Action<int> OnTakeDamage;
        public event Action<Enemy> OnDie;

        [SerializeField] private EnemyData _enemyData;

        private EnemyAnimation _enemyAnimation;
        private PlayerStats _playerStats;

        private int _health;
        public EnemyData EnemyData => _enemyData;

        private void OnEnable()
        {
            _enemyAnimation = GetComponent<EnemyAnimation>();
            _playerStats = FindObjectOfType<PlayerStats>();

            BattleState.OnTurnIncremented += DealDamage;

            _health = _enemyData.Health;
        }

        private void DealDamage(int turn)
        {
            if (turn < 0 || turn % _enemyData.Periodicity != 0) return;

            Debug.Log("Damage");
            _enemyAnimation.PlayAttackAnimation();
            _playerStats.TakeDamage(_enemyData.Damage);
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            _enemyAnimation.PlayGetHitAnimation();

            _health -= damage;
            OnTakeDamage?.Invoke(_health);

            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log("Die");
            BattleState.Instance.RemoveEnemy(this);
            OnDie?.Invoke(this);

            Destroy(gameObject);
        }

        private void OnDisable()
        {
            BattleState.OnTurnIncremented -= DealDamage;
        }
    }
}