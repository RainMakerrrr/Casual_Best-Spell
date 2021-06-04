using GameEconomy;
using Items;
using UnityEngine;

namespace GameMap.Quests
{
    [CreateAssetMenu(fileName = "New Find Item Quest", menuName = "Quests / Find Item Quest")]
    public class FindItemQuest : Quest
    {
        [SerializeField] private QuestItem _questItem;

        public QuestItem QuestItem => _questItem;

        public override void EvaluateQuest()
        {
            Debug.Log("Find Item Quest Executed");
            IsExecuted = true;
            PlayerQuests.Instance.AddItem(_questItem);
        }

        public override void TakeReward()
        {
            if (!PlayerQuests.Instance.RemoveItem(_questItem, this)) return;

            var playerGold = FindObjectOfType<PlayerGold>();
            playerGold.AddGold(QuestReward);

            Debug.Log($"You got {QuestReward}");
        }

        public void CheckQuestItem(QuestItem questItem)
        {
            if (_questItem != questItem) return;

            EvaluateQuest();
        }
    }
}