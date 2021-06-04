using System.Collections.Generic;
using System.Linq;
using Data.Spells;
using DG.Tweening;
using UnityEngine;

namespace DeckBuilding
{
    public class PlayerDeck : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private List<SpellCard> _spellCards = new List<SpellCard>();

        private readonly List<Vector3> _spellPositions = new List<Vector3>();

        private void Start()
        {
            _spellCards = GetComponentsInChildren<SpellCard>().ToList();
            _spellPositions.AddRange(_spellCards.Select(s => s.transform.position));

            InitializeCollection();
        }

        private void InitializeCollection()
        {
            var duplicates = _deck.CurrentDeck
                .GroupBy(x => x)
                .SelectMany(g => g.Skip(1))
                .ToList();

            var distinctCollection = _deck.CurrentDeck.Distinct().ToList();

            FindDuplicates(duplicates, distinctCollection);

            if (distinctCollection.Count <= 0)
            {
                _spellCards.ForEach(card => card.gameObject.SetActive(false));
                return;
            }

            for (int i = 0; i < distinctCollection.Count; i++)
            {
                _spellCards[i].SetSpellData(distinctCollection[i]);
            }

            foreach (var card in _spellCards.Where(spellCard => !spellCard.HasSpellData))
            {
                card.gameObject.SetActive(false);
            }
        }

        private void FindDuplicates(IReadOnlyCollection<SpellData> duplicates, IReadOnlyCollection<SpellData> spells)
        {
            if (duplicates.Count <= 0) return;

            foreach (var duplicate in duplicates.Where(duplicate => spells.Any(spell => duplicate == spell)))
            {
                duplicate.DuplicatesCount++;
            }

            _deck.RemoveCollectionFromDeck(duplicates);
        }

        public void RepositionCards()
        {
            var activeSpells = _spellCards.Where(spell => spell.gameObject.activeSelf).ToList();
            var disabledSpells = _spellCards.Where(spell => !spell.gameObject.activeSelf).ToList();

            var spells = activeSpells.Union(disabledSpells).ToList();

            for (int i = 0; i < spells.Count; i++)
            {
                spells[i].transform.DOMove(_spellPositions[i], 0.5f);
            }
        }

        public Transform GetDisableSpellTransform(SpellCard spellCard, out int index)
        {
            var disabledSpell = _spellCards.FirstOrDefault(card => !card.gameObject.activeSelf);
            if (disabledSpell == null)
            {
                var sameSpell = _spellCards.FirstOrDefault(data => spellCard.SpellData == data.SpellData);
                if (sameSpell == null)
                {
                    index = -1;
                    return null;
                }

                sameSpell.SpellData.DuplicatesCount++;
                spellCard.gameObject.SetActive(false);

                index = -1;
                return null;
            }

            index = _spellCards.IndexOf(disabledSpell);
            _spellCards.Remove(disabledSpell);

            return disabledSpell.transform;
        }


        public void AddSpellCard(SpellCard spellCard, int index) => _spellCards.Insert(index, spellCard);

        public void RemoveSpellCard(SpellCard spellCard) => _spellCards.Remove(spellCard);
    }
}