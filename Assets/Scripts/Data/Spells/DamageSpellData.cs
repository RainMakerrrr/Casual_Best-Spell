using UnityEngine;

namespace Data.Spells
{
    [CreateAssetMenu(fileName = "New Damage Spell", menuName = "Data / Spell Data / Damage Spell")]
    public class DamageSpellData : SpellData
    {
        [SerializeField] private int _damageValue;
        [SerializeField] private DamageType _damageType;


        public int DamageValue => _damageValue;
        public DamageType DamageType => _damageType;


        public override void UpgradeSpell()
        {
            base.UpgradeSpell();

            _damageValue++;
        }
    }
}