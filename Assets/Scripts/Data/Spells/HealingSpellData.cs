using UnityEngine;

namespace Data.Spells
{
    [CreateAssetMenu(fileName = "New Damage Spell", menuName = "Data / Spell Data / Healing Spell")]
    public class HealingSpellData : SpellData
    {
        [SerializeField] private int _healingValue;

        public int HealingValue => _healingValue;

        public override void UpgradeSpell()
        {
            base.UpgradeSpell();

            _healingValue++;
        }
    }
}