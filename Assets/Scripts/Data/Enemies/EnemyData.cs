using System;
using UnityEngine;

namespace Data.Enemies
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Data / Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private int _damage;
        [SerializeField] private int _periodicity;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private EnemyType _enemyType;
        
        public int Health => _health;
        public int Damage => _damage;
        public int Periodicity => _periodicity;

        public DamageType DamageType => _damageType;
        public EnemyType EnemyType => _enemyType;

    }
}