using System;
using System.Collections.Generic;
using Data.Figures;
using SaveSystem;
using SpellsFactory;
using UnityEngine;

namespace Data.Spells
{
    [CreateAssetMenu(fileName = "New Spell Data", menuName = "Data / Spell Data / Common Spell")]
    public class SpellData : ScriptableObject
    {
        public event Action<int> OnDuplicatesChanged;

        [SerializeField] private string _name;
        [SerializeField] private int _duplicatesCount;
        [SerializeField] private Sprite _spellIcon;
        [SerializeField] private List<FigureData> _spellRecipe = new List<FigureData>();

        private SaveData _saveData;

        public int CurrentUpgradeLevel;

        private readonly int[] _upgradePerLevel =
        {
            3, 6, 9, 12
        };

        private readonly int[] _upgradeCostPerLevel =
        {
            100, 200, 400, 600
        };

        public int CurrentTargetDuplicates;
        public int CurrentUpgradeCost;

        public bool EnoughDuplicates => _duplicatesCount >= CurrentTargetDuplicates;

        public int DuplicatesCount
        {
            get => _duplicatesCount;
            set
            {
                _duplicatesCount = value;
                OnDuplicatesChanged?.Invoke(_duplicatesCount);
            }
        }


        public string Name => _name;

        public Sprite SpellIcon => _spellIcon;

        public List<FigureData> SpellRecipe => _spellRecipe;

        public SpellEffect SpellEffect { get; private set; }


        [ContextMenu("Save")]
        private void Save()
        {
            _saveData._name = _name;
            _saveData._duplicatesCount = _duplicatesCount;
            _saveData._spellRecipe = _spellRecipe;
            _saveData._currentUpgradeLevel = CurrentUpgradeLevel;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _name = _saveData._name;
            _duplicatesCount = _saveData._duplicatesCount;
            _spellRecipe = _saveData._spellRecipe;
            CurrentUpgradeLevel = _saveData._currentUpgradeLevel;
        }

        private void OnEnable()
        {
            // if (!JsonSaveSystem.IsFileExist(name))
            // {
            //     Save();
            // }
            //
            // Load();

            CurrentTargetDuplicates = _upgradePerLevel[CurrentUpgradeLevel];
            CurrentUpgradeCost = _upgradeCostPerLevel[CurrentUpgradeLevel];
            SpellEffect = SpellEffectFactory.GetSpellEffect(_name);
        }

        public virtual void UpgradeSpell()
        {
            CurrentUpgradeLevel++;

            CurrentTargetDuplicates = _upgradePerLevel[CurrentUpgradeLevel];
            CurrentUpgradeCost = _upgradeCostPerLevel[CurrentUpgradeLevel];
        }

        // private void OnDisable()
        // {
        //     Save();
        // }


        [Serializable]
        protected struct SaveData
        {
            public string _name;
            public int _duplicatesCount;
            public List<FigureData> _spellRecipe;
            public int _currentUpgradeLevel;
            public int _effectValue;
        }
    }
}