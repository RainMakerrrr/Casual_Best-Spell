using UnityEngine;

namespace SpellsFactory
{
    public class LivingArmor : HealingSpellEffect
    {
        public override string Name => "Living Armor";

        public override void Process()
        {
            Debug.Log("Living Armor");

            var playerStats = FindPlayerStats();
            if (playerStats == null) return;

            playerStats.IncreaseArmor(HealingSpellData.HealingValue);
        }

        public override Vector3? GetPosition()
        {
            var playerStats = FindPlayerStats();
            if (playerStats == null) return null;

            var targetPosition = playerStats.SpellTarget.position;

            return targetPosition;
        }
    }
}