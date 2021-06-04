using System.Collections.Generic;
using System.Linq;
using Data.Spells;
using Enemies;
using Player;
using UnityEngine;

namespace SpellsFactory
{
    public abstract class SpellEffect
    {
        public abstract string Name { get; }

        public abstract void Process();
        public abstract Vector3? GetPosition();
    }

    public abstract class HealingSpellEffect : SpellEffect
    {
        protected HealingSpellData HealingSpellData => Resources.Load<HealingSpellData>($"Data/Spells/{Name}");
        
        protected static PlayerStats FindPlayerStats() => Object.FindObjectOfType<PlayerStats>();
    }

    public abstract class DamageSpellEffect : SpellEffect
    {
        protected DamageSpellData DamageSpellData => Resources.Load<DamageSpellData>($"Data/Spells/{Name}");
        protected List<Enemy> Damagables { get; private set; } = new List<Enemy>();

        protected void InitializeDamagableTargets()
        {
            // _damagables = Object.FindObjectsOfType<GameObject>()
            //     .Select(g => g.GetComponent<IDamagable>())
            //     .Where(d => d != null)
            //     .ToList();

            Damagables = Object.FindObjectsOfType<Enemy>().ToList();
        }

        protected Enemy FindDamagableTarget()
        {
            InitializeDamagableTargets();

            if (Damagables.Count == 0) return null;

            var damagable = Damagables.FirstOrDefault();
            return damagable;
        }
    }
}