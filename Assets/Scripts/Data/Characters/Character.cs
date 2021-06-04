using System;
using System.Collections.Generic;
using System.Linq;
using GameMap.Locations;
using GameMap.Quests;
using Items;
using SaveSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Data / Character Data")]
    public class Character : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<Quest> _quests = new List<Quest>();
        [SerializeField] private Sprite _characterSprite;
        [SerializeField] private Sprite _characterIcon;

        private SaveData _saveData;

        public string Name => _name;

        public List<Quest> Quests => _quests;
        public Sprite CharacterSprite => _characterSprite;
        public Sprite CharacterIcon => _characterIcon;

        public bool HasQuests => _quests.All(quest => quest.IsAccepted);
        public bool QuestsCompleted => _quests.All(quest => quest.IsCompleted);

        // private void OnEnable()
        // {
        //     if (JsonSaveSystem.IsFileExist(name))
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
            _saveData._quests = _quests;
            JsonSaveSystem.Save(_saveData, name);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            _saveData = JsonSaveSystem.Load<SaveData>(name);
            _quests = _saveData._quests;
        }

        public Quest GetQuest()
        {
            var quest = _quests.FirstOrDefault(q => !q.IsAccepted);
            return quest;
        }

        public void CheckEnemiesQuests(LocationData locationData, int count)
        {
            var enemiesQuest = new List<DefeatEnemiesQuest>();

            foreach (var quest in _quests)
            {
                if (quest is DefeatEnemiesQuest defeatEnemiesQuest)
                    enemiesQuest.Add(defeatEnemiesQuest);
            }

            if (enemiesQuest.Count <= 0) return;

            foreach (var quest in enemiesQuest.Where(quest => quest.Location == locationData))
            {
                quest.CheckEnemiesCount(count);
            }
        }

        public void CheckItemsQuests(LocationData locationData, QuestItem item)
        {
            var itemQuests = new List<FindItemQuest>();

            foreach (var quest in _quests)
            {
                if (quest is FindItemQuest findItemQuest)
                    itemQuests.Add(findItemQuest);
            }

            if (itemQuests.Count <= 0) return;

            foreach (var itemQuest in itemQuests.Where(quest => quest.Location == locationData))
            {
                itemQuest.CheckQuestItem(item);
            }
        }

        public void CheckFailedQuests(QuestItem item, FindItemQuest itemQuest)
        {
            var itemQuests = new List<FindItemQuest>();

            foreach (var quest in _quests.Except(new List<Quest> {itemQuest}))
            {
                if (quest is FindItemQuest findItemQuest)
                    itemQuests.Add(findItemQuest);
            }

            if (itemQuests.Count <= 0) return;

            foreach (var quest in itemQuests.Where(i => i.QuestItem == item))
            {
                quest.IsFailed = true;
            }
        }

        public void RemoveQuest(Quest quest) => _quests.Remove(quest);

        [Serializable]
        private struct SaveData
        {
            public List<Quest> _quests;
        }
    }
}