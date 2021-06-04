using System.Collections.Generic;
using Data.Characters;
using GameMap.Data;
using GameState;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMap.Quests.UI
{
    public class QuestAccepter : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _questDialogueText;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private Image _questCustomerImage;
        [SerializeField] private MapData _mapData;

        private int _dialogueReplicasCount;
        private Quest _quest;
        private Character _character;

        private List<Character> _tempCharacters = new List<Character>();

        private void Start()
        {
            _tempCharacters.AddRange(_mapData.MapCharacters);
            EnableQuestDialogue();
        }

        private void EnableQuestDialogue()
        {
            if (_tempCharacters.Count <= 0)
                gameObject.SetActive(false);

            foreach (var character in _tempCharacters)
            {
                _character = character;
                _character = _mapData.GetCharacter();
                if (_character == null)
                {
                    continue;
                }


                _quest = _character.GetQuest();
                if (_quest == null)
                {
                    continue;
                }

                _dialogueReplicasCount = 0;

                gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(true);

                _characterName.text = _character.Name;
                _questCustomerImage.sprite = _character.CharacterSprite;

                if (_quest.DialogueReplicas.Length <= 0)
                {
                    AcceptQuest();
                    gameObject.SetActive(false);
                    continue;
                }

                _questDialogueText.text = _quest.DialogueReplicas[_dialogueReplicasCount];
            }
        }

        private void AcceptQuest()
        {
            PlayerQuests.Instance.AddCharacter(_character);

            _quest.IsAccepted = true;
            _mapData.RemoveCharacter(_character);

            gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_character == null || _quest == null) return;

            _dialogueReplicasCount++;
            if (_dialogueReplicasCount >= _quest.DialogueReplicas.Length)
            {
                AcceptQuest();
                EnableQuestDialogue();
                return;
            }

            _questDialogueText.text = _quest.DialogueReplicas[_dialogueReplicasCount];
        }
    }
}