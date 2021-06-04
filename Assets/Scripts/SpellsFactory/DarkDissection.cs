using UnityEngine;

namespace SpellsFactory
{
    public class DarkDissection : DamageSpellEffect
    {
        public override string Name => "Dark Dissection";

        public override void Process()
        {
            Debug.Log("Dark Dissection");

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