using System;
using SaveSystem;
using UnityEngine;

namespace GameEconomy.Data
{
    [CreateAssetMenu(fileName = "New Gold Data", menuName = "Economy / Gold")]
    public class Gold : ScriptableObject
    {
        [SerializeField] private int _playerGold = 500;
        private SaveData _saveData;


        private void OnEnable()
        {
            if (!JsonSaveSystem.IsFileExist(name))
            {
                Save();
            }
            Load();
        }
        
        private void OnDisable()
        {
            Save();
        }

        [ContextMenu("Save")]
        private void Save()
        {
            _saveData.Gold = _playerGold;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _playerGold = _saveData.Gold;
        }


        public int PlayerGold
        {
            get => _playerGold;
            set => _playerGold = value;
        }

        public void AddGold(int gold) => _playerGold += gold;

        public bool RemoveGold(int gold)
        {
            if (gold > _playerGold)
            {
                Debug.Log("Not enough gold");
                return false;
            }

            _playerGold -= gold;
            return true;
        }
    }

    [Serializable]
    public struct SaveData
    {
        public int Gold;
    }
}