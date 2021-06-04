using System;
using Data.Spells;
using DeckBuilding.UI;
using DG.Tweening;
using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class SpellCard : MonoBehaviour
    {
        [SerializeField] private SpellsPool _spellsPool;
        [SerializeField] private Deck _deck;

        [SerializeField] private Slider _duplicatesBar;
        [SerializeField] private Image _duplicatesPanel;
        [SerializeField] private TextMeshProUGUI _duplicatesText;

        private SpellView _spellView;
        private PlayerDeck _playerDeck;
        private CardsCollection _cardsCollection;
        private SpellData _spellData;

        public SpellData SpellData => _spellData;
        public bool HasSpellData => _spellData != null;

        private Vector3 _startPosition;

        private void OnEnable()
        {
            _startPosition = transform.position;
            _spellView = GetComponent<SpellView>();
            
            _playerDeck = FindObjectOfType<PlayerDeck>();
            _cardsCollection = FindObjectOfType<CardsCollection>();

            SpellInfo.OnSpellUpgraded += UpdateSpellData;
        }

        private void UpdateDuplicatesView(int duplicates)
        {
            _duplicatesText.text = $"{duplicates}/{_spellData.CurrentTargetDuplicates}";
        }

        private void UpdateSpellData(SpellData spellData)
        {
            if (_spellData != spellData) return;

            _duplicatesBar.maxValue = _spellData.CurrentTargetDuplicates;
            _duplicatesBar.value = _spellData.DuplicatesCount;

            _duplicatesPanel.color = _spellData.EnoughDuplicates
                ? Color.green
                : Color.magenta;
            _duplicatesText.text = $"{_spellData.DuplicatesCount}/{_spellData.CurrentTargetDuplicates}";
        }

        public void SetSpellData(SpellData spellData)
        {
            _spellData = spellData;
            _spellData.OnDuplicatesChanged += UpdateDuplicatesView;

            _spellView.SetIcons(_spellData);

            _duplicatesBar.maxValue = _spellData.CurrentTargetDuplicates;
            _duplicatesBar.value = _spellData.DuplicatesCount;

            _duplicatesPanel.color = _spellData.EnoughDuplicates
                ? Color.green
                : Color.magenta;
            _duplicatesText.text = $"{_spellData.DuplicatesCount}/{_spellData.CurrentTargetDuplicates}";
        }

        public void MoveToDeck()
        {
            var target = _playerDeck.GetDisableSpellTransform(this, out var index);
            if (target == null) return;

            transform.SetParent(target.parent);

            transform.DOMove(target.position, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _deck.AddSpellToDeck(_spellsPool.SpellsCollection, _spellData);
                    _spellsPool.RemoveSpellFromPool(_spellData);

                    _playerDeck.AddSpellCard(this, index);
                    _cardsCollection.AddSpellCard(target.GetComponent<SpellCard>(), index);
                    _cardsCollection.RemoveSpellCard(this);

                    target.SetParent(_cardsCollection.transform.GetChild(0).GetChild(0));
                    target.transform.position = _startPosition;

                    _startPosition = transform.position;
                    _cardsCollection.RepositionCards();
                });
        }

        public void MoveToCollection()
        {
            var target = _cardsCollection.GetDisabledSpellTransform(this, out var index);
            if (target == null) return;

            transform.SetParent(target.parent);

            transform.DOMove(target.position, 0.5f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _spellsPool.AddSpellToPool(_deck.CurrentDeck, _spellData);
                    _deck.RemoveSpellFromDeck(_spellData);

                    _cardsCollection.AddSpellCard(this, index);
                    _playerDeck.AddSpellCard(target.GetComponent<SpellCard>(), index);
                    _playerDeck.RemoveSpellCard(this);

                    target.SetParent(_playerDeck.transform.GetChild(0).GetChild(0));
                    target.transform.position = _startPosition;

                    _startPosition = transform.position;

                    _playerDeck.RepositionCards();
                });
        }

        private void OnDisable()
        {
            SpellInfo.OnSpellUpgraded -= UpdateSpellData;

            if (GetComponentInParent<PlayerDeck>() != null)
                _deck.RemoveSpellFromDeck(_spellData);
            else if (GetComponentInParent<CardsCollection>() != null)
                _spellsPool.RemoveSpellFromPool(_spellData);
        }

        private void OnApplicationQuit()
        {
            _deck.Save();
            _spellsPool.Save();
        }
    }
}