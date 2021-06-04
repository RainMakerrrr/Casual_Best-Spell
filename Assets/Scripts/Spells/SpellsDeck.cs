using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Spells;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Spells
{
    public class SpellsDeck : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private List<Spell> _spells = new List<Spell>();
        [SerializeField] private Spell _spellPrefab;

        private List<SpellData> _playingDeck = new List<SpellData>();
        public List<Spell> Spells => _spells;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            _spells = FindObjectsOfType<Spell>().ToList();
            _playingDeck.AddRange(_deck.CurrentDeck.Distinct());
            RandomizeSpells();
        }


        private void RandomizeSpells()
        {
            foreach (var spell in _spells)
            {
                var randomSpell = _playingDeck[Random.Range(0, _playingDeck.Count)];
                spell.SpellData = randomSpell;
                spell.SpellView.SetIcons(spell.SpellData);
                _playingDeck.Remove(randomSpell);
            }
        }

        public void RemoveSpell(Spell spell)
        {
            _spells.Remove(spell);
            _playingDeck.Add(spell.SpellData);

            var newSpell = Instantiate(_spellPrefab, transform.position, Quaternion.identity);
            newSpell.transform.SetParent(transform);
            newSpell.transform.localScale = Vector3.one;

            var spellPool = _playingDeck.Where(s => s != spell.SpellData).ToList();
            newSpell.SpellData = Random.Range(0, 101) > 98
                ? spell.SpellData
                : spellPool[Random.Range(0, spellPool.Count)];

            newSpell.transform.DOMove(spell.StartPosition, 0.8f)
                .SetEase(Ease.Unset)
                .OnComplete(() =>
                {
                    AddSpell(newSpell);
                    _playingDeck.Remove(newSpell.SpellData);
                    newSpell.StartPosition = spell.StartPosition;
                });
        }

        private void AddSpell(Spell spell) => _spells.Add(spell);
        
    }
}