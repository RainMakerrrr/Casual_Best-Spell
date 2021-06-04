using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace Data.Spells
{
    [CreateAssetMenu(fileName = "Spells Pool", menuName = "Spells Pool")]
    public class SpellsPool : ScriptableObject
    {
        [SerializeField] private List<SpellData> _spellsCollection = new List<SpellData>();

        public List<SpellData> SpellsCollection => _spellsCollection;
        
        private SaveData _saveData;

        // private void OnEnable()
        // {
        //     if (!JsonSaveSystem.IsFileExist(name))
        //     {
        //         Save();
        //     }
        //     Load();
        // }
        //
        // private void OnDisable()
        // {
        //     Save();
        // }

        [ContextMenu("Save")]
        public void Save()
        {
            _saveData._spellsCollection = _spellsCollection;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _spellsCollection = _saveData._spellsCollection;
        }

        public void AddSpellToPool(IEnumerable<SpellData> deckSpells, SpellData spellData)
        {
            var duplicates = deckSpells.GroupBy(x => x)
                .SelectMany(g => g.Skip(1)).ToList();

            if (duplicates.Contains(spellData))
            {
                foreach (var duplicate in duplicates)
                {
                    _spellsCollection.Add(duplicate);
                }
            }

            _spellsCollection.Add(spellData);
        }

        public void RemoveSpellFromPool(SpellData spellData)
        {
            var duplicates = _spellsCollection.GroupBy(x => x)
                .SelectMany(g => g.Skip(1)).ToList();

            if (duplicates.Contains(spellData))
            {
                foreach (var duplicate in duplicates)
                {
                    _spellsCollection.Remove(duplicate);
                }
            }

            _spellsCollection.Remove(spellData);
        }

        public void RemoveCollectionFromDeck(IEnumerable<SpellData> spellsData) =>
            _spellsCollection = _spellsCollection.Except(spellsData).ToList();

        [Serializable]
        private struct SaveData
        {
            public List<SpellData> _spellsCollection;
        }
    }
}