using System;
using System.Linq;
using Data.Characters;
using GameMap.Data;
using GameMap.Locations;
using Items;
using UnityEngine;

namespace GameMap.Quests
{
    public class PlayerQuests : MonoBehaviour
    {
        public static event Action<Character> OnCharacterAdded;
        public static event Action<Character> OnCharacterRemoved;

        [SerializeField] private MapData _mapData;
        [SerializeField] private PlayerItems _playerItems;

        public static PlayerQuests Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(this);
        }

        public void CheckDefeatEnemiesQuests(LocationData locationData, int count)
        {
            _mapData.AcceptedCharacters.ForEach(character => character.CheckEnemiesQuests(locationData, count));
        }

        public void CheckFindItemQuests(LocationData locationData, QuestItem item)
        {
            _mapData.AcceptedCharacters.ForEach(character => character.CheckItemsQuests(locationData, item));
        }

        public void AddItem(QuestItem item)
        {
            if (!_playerItems.Items.Contains(item))
                _playerItems.Items.Add(item);
        }

        public bool RemoveItem(QuestItem item, FindItemQuest itemQuest)
        {
            if (!_playerItems.Items.Contains(item)) return false;

            _playerItems.Items.Remove(item);
            _mapData.AcceptedCharacters.ForEach(character => character.CheckFailedQuests(item, itemQuest));

            return true;
        }

        public void RemoveCharacter(Character character)
        {
            _mapData.AcceptedCharacters.Remove(character);
            OnCharacterRemoved?.Invoke(character);
        }

        public void AddCharacter(Character character)
        {
            if (_mapData.AcceptedCharacters.Contains(character)) return;

            _mapData.AcceptedCharacters.Add(character);
            OnCharacterAdded?.Invoke(character);
        }
    }
}