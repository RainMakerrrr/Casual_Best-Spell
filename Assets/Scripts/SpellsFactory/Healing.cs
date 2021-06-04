using UnityEngine;

namespace SpellsFactory
{
    public class Healing : HealingSpellEffect
    {
        public override string Name => "Healing";

        public override void Process()
        {
            Debug.Log("Healing");

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