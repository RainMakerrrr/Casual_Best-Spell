using System;
using SaveSystem;
using UnityEngine;

namespace Data.PlayerData
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Data / Player Stats")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private int _armor;

        private SaveData _saveData;

        public int Health => _health;
        public int Armor => _armor;

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
        private void Save()
        {
            _saveData._health = _health;
            _saveData._armor = _armor;

            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);

            _health = _saveData._health;
            _armor = _saveData._armor;
        }


        [Serializable]
        private struct SaveData
        {
            public int _health;
            public int _armor;
        }
    }
}