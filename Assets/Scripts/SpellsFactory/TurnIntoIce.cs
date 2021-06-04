using UnityEngine;

namespace SpellsFactory
{
    public class TurnIntoIce : DamageSpellEffect
    {
        public override string Name => "Turn into ice";

        public override void Process()
        {
            Debug.Log("Turn into ice");

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