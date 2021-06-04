using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap.Quests.UI
{
    public class QuestPanel : MonoBehaviour
    {
        public static event Action<Quest> OnQuestCompleted;
        public static event Action<Quest> OnQuestFailed;

        [SerializeField] private TextMeshProUGUI _questName;
        [SerializeField] private TextMeshProUGUI _questDescription;
        [SerializeField] private TextMeshProUGUI _questQuantity;
        [SerializeField] private TextMeshProUGUI _goldRewardQuantity;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _executeQuestButton;

        [SerializeField] private Sprite _completedQuestSprite;
        [SerializeField] private Sprite _executingQuestSprite;
        [SerializeField] private Sprite _failedQuestSprite;

        private Image _buttonImage;

        private Quest _quest;

        public bool HasQuest => _quest != null;

        private void OnEnable()
        {
            _buttonImage = _executeQuestButton.GetComponent<Image>();
            _executeQuestButton.onClick.AddListener(ExecuteQuest);
        }

        public void SetQuest(Quest quest)
        {
            _quest = quest;
            UpdateQuestUI();
        }

        private void UpdateQuestUI()
        {
            if (_quest == null) return;

            _questName.text = _quest.QuestName;
            _questDescription.text = _quest.Description;

            switch (_quest)
            {
                case DefeatEnemiesQuest enemiesQuest:
                    _questQuantity.text = $"0/{enemiesQuest.EnemiesToDefeat}";
                    _progressBar.value = 0;
                    _progressBar.maxValue = enemiesQuest.EnemiesToDefeat;
                    break;
                case FindItemQuest findItemQuest:
                    _questQuantity.text = "0/1";
                    _progressBar.value = 0;
                    _progressBar.maxValue = 1;
                    break;
            }

            _goldRewardQuantity.text = _quest.QuestReward.ToString();
            SetButtonSprite();
        }

        private void ExecuteQuest()
        {
            if (_quest == null) return;
            if (!_quest.IsExecuted) return;

            if (_quest.IsFailed)
            {
                OnQuestFailed?.Invoke(_quest);
                return;
            }

            Debug.Log("Execute quest" + _quest.QuestName);
            Debug.Log(name);
            _quest.IsCompleted = true;
            OnQuestCompleted?.Invoke(_quest);
        }

        private void SetButtonSprite()
        {
            if (_quest == null) return;


            if (_quest.IsFailed)
            {
                _executeQuestButton.interactable = true;
                _buttonImage.sprite = _failedQuestSprite;
                return;
            }

            if (_quest.IsExecuted)
            {
                _executeQuestButton.interactable = true;
                _buttonImage.sprite = _completedQuestSprite;
            }
            else
            {
                _executeQuestButton.interactable = false;
                _buttonImage.sprite = _executingQuestSprite;
            }
        }

        private void OnDisable()
        {
            _executeQuestButton.onClick.RemoveListener(ExecuteQuest);
        }
    }
}