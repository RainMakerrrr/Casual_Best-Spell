using System.Collections.Generic;
using Data.Figures;
using Data.Spells;
using DG.Tweening;
using UnityEngine;

namespace Spells
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] private SpellData _spellData;
        private readonly List<FigureData> _spellRecipe = new List<FigureData>();

        private SpellsDeck _spellsDeck;
        private SpellView _spellView;

        public SpellData SpellData
        {
            get => _spellData;
            set
            {
                if (value != null)
                    _spellData = value;
            }
        }

        public SpellView SpellView => _spellView;
        public int SpellCost => _spellView.UsedImages.Count;
        public Vector3 StartPosition { get; set; }

        private void Start()
        {
            _spellsDeck = FindObjectOfType<SpellsDeck>();
            _spellView = GetComponent<SpellView>();

            StartPosition = transform.position;
            _spellView.SetIcons(_spellData);
        }

        public void AddRecipeElement(FigureData element)
        {
            _spellRecipe.Add(element);

            if (_spellRecipe.Count < _spellData.SpellRecipe.Count) return;

            CastSpell();
        }

        private void CastSpell()
        {
            _spellView.Background.SetAlpha(0f);

            var target = _spellData.SpellEffect.GetPosition();
            if (target == null) return;

            _spellView.SpellIcon.transform.DOMove(target.Value, 0.2f)
                .SetEase(Ease.Unset)
                .OnComplete(() =>
                {
                    _spellData.SpellEffect?.Process();
                    _spellsDeck.RemoveSpell(this);
                    Destroy(gameObject);
                });
        }
    }
}