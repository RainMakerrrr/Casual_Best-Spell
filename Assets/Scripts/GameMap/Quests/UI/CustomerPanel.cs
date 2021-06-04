using System.Linq;
using Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMap.Quests.UI
{
    public class CustomerPanel : MonoBehaviour
    {
        [SerializeField] private Character _character;

        private Image _questCustomerIcon;
        private Button _questPanelButton;

        private Sprite _customerSprite;

        private CharacterInfoPanel _characterInfoPanel;

        private int _questIndex;
        public bool IsPanelBusy { get; private set; }

        private void OnEnable()
        {
            _questCustomerIcon = GetComponent<Image>();
            _questPanelButton = GetComponent<Button>();

            _characterInfoPanel = FindObjectOfType<CharacterInfoPanel>(true);

            _customerSprite = _questCustomerIcon.sprite;

            _questPanelButton.onClick.AddListener(PanelClickHandler);
            //  _nextQuestPanel.onClick.AddListener(IncrementQuestIndex);

            PlayerQuests.OnCharacterRemoved += ClearCharacterPanel;
        }

        private void ClearCharacterPanel(Character character)
        {
            if (_character != character) return;

            _character = null;
            _questCustomerIcon.sprite = _customerSprite;
            IsPanelBusy = false;
        }

        public void SetCharacter(Character character)
        {
            if (character == null) return;
            _character = character;
            _questCustomerIcon.sprite = _character.CharacterIcon;
            IsPanelBusy = true;
        }

        private void PanelClickHandler()
        {
            if (_character == null) return;

            _characterInfoPanel.gameObject.SetActive(!_characterInfoPanel.gameObject.activeSelf);
            _characterInfoPanel.SetCharacterData(_character);

            // _descriptionPanel.gameObject.SetActive(!_descriptionPanel.gameObject.activeSelf);
            // _descriptionText.text = _character.Quests[_questIndex]?.Description;
            //
            // _executeQuestButton.gameObject.SetActive(_character.Quests[_questIndex].IsExecuted);
        }

        // private void ExecuteButton()
        // {
        //     var completedQuest = _character.Quests.FirstOrDefault(quest => quest.IsExecuted);
        //     if (completedQuest == null) return;
        //
        //     completedQuest.IsCompleted = true;
        //     _character.RemoveQuest(completedQuest);
        //
        //     if (_character.QuestsCompleted)
        //     {
        //         PlayerQuests.Instance.RemoveCharacter(_character);
        //         _descriptionPanel.gameObject.SetActive(false);
        //         return;
        //     }
        //
        //     _questIndex = 0;
        //     _descriptionText.text = _character.Quests[_questIndex]?.Description;
        //     _executeQuestButton.gameObject.SetActive(_character.Quests[_questIndex].IsExecuted);
        // }

        // private void IncrementQuestIndex()
        // {
        //     _questIndex++;
        //     if (_questIndex >= _character.Quests.Count)
        //         _questIndex = 0;
        //
        //     _descriptionText.text = _character.Quests[_questIndex]?.Description;
        //     _executeQuestButton.gameObject.SetActive(_character.Quests[_questIndex].IsExecuted);
        // }

        private void OnDisable()
        {
            _questPanelButton.onClick.RemoveListener(PanelClickHandler);
            // _nextQuestPanel.onClick.RemoveListener(IncrementQuestIndex);

            PlayerQuests.OnCharacterRemoved -= ClearCharacterPanel;
        }
    }
}