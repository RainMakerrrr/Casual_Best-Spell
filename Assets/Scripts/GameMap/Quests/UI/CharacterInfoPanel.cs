using System.Collections.Generic;
using System.Linq;
using Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap.Quests.UI
{
    public class CharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private List<QuestPanel> _questPanels = new List<QuestPanel>();

        [SerializeField] private Image _characterIcon;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private Button _closeButton;

        private Character _character;

        private void Start()
        {
            _questPanels = GetComponentsInChildren<QuestPanel>(true).ToList();

            QuestPanel.OnQuestCompleted += QuestCompleteHandler;
            QuestPanel.OnQuestFailed += QuestFailedHandler;

            // foreach (var questPanel in _questPanels)
            // {
            //     questPanel.OnQuestCompleted += QuestCompleteHandler;
            //     questPanel.OnQuestFailed += QuestFailedHandler;
            // }

            _closeButton.onClick.AddListener(DisablePanel);
        }

        private void DisablePanel() => gameObject.SetActive(false);

        public void SetCharacterData(Character character)
        {
            _character = character;
            UpdateDataUI();
        }

        private void QuestCompleteHandler(Quest completedQuest)
        {
            if (_character == null) return;

            Debug.Log("Quest complete " + completedQuest.QuestName);
            _character.RemoveQuest(completedQuest);


            if (_character.QuestsCompleted)
            {
                PlayerQuests.Instance.RemoveCharacter(_character);
                gameObject.SetActive(false);
                return;
            }

            UpdateDataUI();
        }

        private void QuestFailedHandler(Quest failedQuest)
        {
            Debug.Log("Quest failed" + failedQuest.QuestName);

            _character.RemoveQuest(failedQuest);


            if (_character.QuestsCompleted)
            {
                PlayerQuests.Instance.RemoveCharacter(_character);
                gameObject.SetActive(false);
                return;
            }

            UpdateDataUI();
        }

        private void UpdateDataUI()
        {
            _questPanels = new List<QuestPanel>();
            _questPanels = GetComponentsInChildren<QuestPanel>(true).ToList();

            _questPanels.ForEach(panel => panel.gameObject.SetActive(false));

            _characterIcon.sprite = _character.CharacterIcon;
            _characterName.text = _character.Name;

            for (int i = 0; i < _character.Quests.Count; i++)
            {
                _questPanels[i].gameObject.SetActive(true);
                _questPanels[i].SetQuest(_character.Quests[i]);
            }

            foreach (var panel in _questPanels.Where(q => !q.HasQuest))
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}