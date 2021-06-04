using System;
using System.Collections.Generic;
using System.Linq;
using Data.Characters;
using Newtonsoft.Json;
using NuclearBand;
using SaveSystem;
using UnityEngine;

#nullable enable

namespace GameMap.Data
{
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Data / Map Data")]
    public class MapData : DataNode
    {
        public string Path = "Map2";
        public const string FolderPath = "Map Data";

        [SerializeField] private List<Character> _mapCharacters = new List<Character>();

        [SerializeField] private List<Character> _acceptedCharacters = new List<Character>();
        [SerializeField, JsonProperty] private int _unlockedLevels;

        private SaveData _saveData;

        [JsonProperty] public int UnlockedLevels => _unlockedLevels;
        public List<Character> MapCharacters => _mapCharacters;

        public List<Character> AcceptedCharacters => _acceptedCharacters;


        private void OnEnable()
        {
            Save();

            Load();
        }

        private void OnDisable()
        {
            Save();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            _saveData._mapCharacters = _mapCharacters;
            _saveData._acceptedCharacters = _acceptedCharacters;
            _saveData._unlockedLevels = _unlockedLevels;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _mapCharacters = _saveData._mapCharacters;
            _acceptedCharacters = _saveData._acceptedCharacters;
            _unlockedLevels = _saveData._unlockedLevels;
        }


        public void SetUnlockedLevel(int value) => _unlockedLevels = value;


        public Character GetCharacter()
        {
            var character = _mapCharacters.FirstOrDefault();
            return character;
        }

        public void RemoveCharacter(Character character)
        {
            if (character.HasQuests)
                _mapCharacters.Remove(character);
        }

        [Serializable]
        private struct SaveData
        {
            public List<Character> _mapCharacters;
            public List<Character> _acceptedCharacters;
            public int _unlockedLevels;
        }
    }
}