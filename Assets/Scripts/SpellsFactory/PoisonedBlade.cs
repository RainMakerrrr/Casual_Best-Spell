using Data;
using UnityEngine;

namespace SpellsFactory
{
    public class PoisonedBlade : DamageSpellEffect
    {
        public override string Name => "Poisoned Blade";

        public override void Process()
        {
            Debug.Log("Poisoned Blade");

            var damagable = FindDamagableTarget();
            if (damagable == null) return;
            
            damagable.TakeDamage(DamageSpellData.DamageValue, DamageSpellData.DamageType);
        }

        public override Vector3? GetPosition()
        {
            var damagable = FindDamagableTarget();
            if (damagable == null) return null;
            
            Vector3? targetPosition = Camera.main.WorldToScreenPoint(damagable.transform.position);

            return targetPosition;
        }
    }
}