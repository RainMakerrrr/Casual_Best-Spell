using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace Data.Spells
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
    public class Deck : ScriptableObject
    {
        [SerializeField] private List<SpellData> _currentDeck = new List<SpellData>();

        public List<SpellData> CurrentDeck => _currentDeck;

        private SaveData _saveData;

        // private void OnEnable()
        // {
        //     if (!JsonSaveSystem.IsFileExist(name))
        //     {
        //         Save();
        //     }
        //     
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
            _saveData._currentDeck = _currentDeck;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _currentDeck = _saveData._currentDeck;
        }
        
        public void AddSpellToDeck(IEnumerable<SpellData> spellsPool, SpellData spellData)
        {
            var duplicates = spellsPool.GroupBy(x => x)
                .SelectMany(g => g.Skip(1)).ToList();

            if (duplicates.Contains(spellData))
            {
                foreach (var duplicate in duplicates)
                {
                    _currentDeck.Add(duplicate);
                }
            }

            _currentDeck.Add(spellData);
        }

        public void RemoveSpellFromDeck(SpellData spellData)
        {
            var duplicates = _currentDeck.GroupBy(x => x)
                .SelectMany(g => g.Skip(1)).ToList();

            if (duplicates.Contains(spellData))
            {
                foreach (var duplicate in duplicates)
                {
                    _currentDeck.Remove(duplicate);
                }
            }

            _currentDeck.Remove(spellData);
        }

        public void RemoveCollectionFromDeck(IEnumerable<SpellData> spellsData) =>
            _currentDeck = _currentDeck.Except(spellsData).ToList();

        [Serializable]
        private struct SaveData
        {
            public List<SpellData> _currentDeck;
        }
    }
    
}