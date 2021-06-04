using GameEconomy;
using SaveSystem;
using UnityEngine;

namespace GameMap.Quests
{
    [CreateAssetMenu(fileName = "New defeat enemies quest", menuName = "Quests / Defeat Enemies Quest")]
    public class DefeatEnemiesQuest : Quest
    {
        [SerializeField] private int _enemiesToDefeat;
        public int EnemiesToDefeat => _enemiesToDefeat;


        public override void EvaluateQuest()
        {
            Debug.Log("Complete quest");

            IsExecuted = true;
        }

        public override void TakeReward()
        {
            var playerGold = FindObjectOfType<PlayerGold>();
            playerGold.AddGold(QuestReward);

            Debug.Log($"You got {QuestReward}");
        }

        public void CheckEnemiesCount(int count)
        {
            if (count < _enemiesToDefeat) return;

            EvaluateQuest();
        }
    }
}