using System;
using System.Collections.Generic;
using System.Linq;
using Data.Figures;
using Data.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Spells
{
    public class SpellView : MonoBehaviour
    {
        [SerializeField] private List<Image> _figuresImages = new List<Image>();
        [SerializeField] private Image _spellIcon;

        private Image _background;
        private List<Image> _usedImages = new List<Image>();

        public Image SpellIcon => _spellIcon;
        public Image Background => _background;
        public List<Image> UsedImages => _usedImages;

        private void Start()
        {
            _background = GetComponent<Image>();
        }

        public void SetIcons(SpellData spellData)
        {
            ClearImages();
            _figuresImages.ForEach(image => image.gameObject.SetActive(true));

            _spellIcon.sprite = spellData.SpellIcon;

            for (int i = 0; i < spellData.SpellRecipe.Count; i++)
            {
                _figuresImages[i].sprite = spellData.SpellRecipe[i].FigureSprite;
                _figuresImages[i].SetAlpha(0.8f);
            }

            _usedImages = _figuresImages.Where(figureImage => figureImage.sprite != null).ToList();

            foreach (var figureImage in _figuresImages.Where(figureImage => figureImage.sprite == null))
            {
                figureImage.gameObject.SetActive(false);
            }
        }

        private void ClearImages()
        {
            _spellIcon.sprite = null;
            _figuresImages.ForEach(figureImage => figureImage.sprite = null);
        }

        public Image GetTargetImage(FigureData figureData)
        {
            var targetImage = _usedImages.Find(usedImage =>
                usedImage.sprite == figureData.FigureSprite || figureData.FigureType == FigureType.Star);
            _usedImages.Remove(targetImage);

            return targetImage;
        }
    }
}