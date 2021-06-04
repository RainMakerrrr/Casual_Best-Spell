using System;
using GameMap.Locations;
using SaveSystem;
using UnityEngine;

namespace GameMap.Quests
{
    public abstract class Quest : ScriptableObject
    {
        [SerializeField] private string _questName;
        [SerializeField] private int _questReward;
        [SerializeField] private LocationData _location;

        [SerializeField] private string _description;
        [SerializeField] private string[] _dialogueReplicas;

        [SerializeField] private bool _isAccepted;
        [SerializeField] private bool _isExecuted;
        [SerializeField] private bool _isCompleted;
        [SerializeField] private bool _isFailed;

        private SaveData _saveData;

        public string QuestName => _questName;

        public int QuestReward => _questReward;
        public LocationData Location => _location;

        public string Description => _description;
        public string[] DialogueReplicas => _dialogueReplicas;

        public bool IsExecuted
        {
            get => _isExecuted;
            set => _isExecuted = value;
        }

        public bool IsAccepted
        {
            get => _isAccepted;
            set => _isAccepted = value;
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                if (_isCompleted)
                    TakeReward();
            }
        }

        public bool IsFailed
        {
            get => _isFailed;
            set => _isFailed = true;
        }


        public abstract void EvaluateQuest();

        public abstract void TakeReward();

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
        protected virtual void Save()
        {
            _saveData._questReward = _questReward;
            _saveData._location = _location;
            _saveData._description = _description;
            _saveData._dialogueReplicas = _dialogueReplicas;
            _saveData._isAccepted = _isAccepted;
            _saveData._isExecuted = _isExecuted;
            _saveData._isCompleted = _isCompleted;

            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        protected virtual void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);

            _questReward = _saveData._questReward;
            _location = _saveData._location;
            _description = _saveData._description;
            _dialogueReplicas = _saveData._dialogueReplicas;
            _isAccepted = _saveData._isAccepted;
            _isExecuted = _saveData._isExecuted;
            _isCompleted = _saveData._isCompleted;
        }


        [Serializable]
        protected struct SaveData
        {
            public int _questReward;
            public LocationData _location;
            public string _description;
            public string[] _dialogueReplicas;

            public bool _isAccepted;
            public bool _isExecuted;
            public bool _isCompleted;

            public int _enemiesToDefeat;
        }
    }
}