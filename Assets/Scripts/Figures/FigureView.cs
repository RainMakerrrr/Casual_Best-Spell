using System;
using Data.Figures;
using UnityEngine;
using UnityEngine.UI;

namespace Figures
{
    public class FigureView : MonoBehaviour
    {
        [SerializeField] private Outline _backgroundOutline;
        [SerializeField] private Outline _iconOutline;

        private Image _figureSprite;
        public Image FigureSprite => _figureSprite;

        private void Awake()
        {
            _iconOutline = GetComponent<Outline>();
            _figureSprite = GetComponent<Image>();
        }

        public void SetActiveOutline(bool isActive) => _iconOutline.enabled = isActive;
        public void UpdateFigureSprite(FigureData figureData) => _figureSprite.sprite = figureData.FigureSprite;

        public void ScaleOutline(bool isLocked)
        {
            _backgroundOutline.enabled = isLocked;
            _backgroundOutline.transform.localScale =
                _backgroundOutline.enabled ? new Vector3(0.8f, 0.8f, 0.8f) : Vector3.one;
        }
    }
}