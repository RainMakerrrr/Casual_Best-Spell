using System;
using Data.Figures;
using GameState;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Figures
{
    public class Figure : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnFigureUnLocked;

        [SerializeField] private FigureData _figureData;

        public FigureData FigureData
        {
            get => _figureData;
            set
            {
                if (value != null)
                    _figureData = value;
            }
        }

        private FigureView _figureView;

        public FigureView FigureView => _figureView;
        public bool IsLocked { get; private set; }
        public bool CanLocked { get; set; } = true;
        public bool IsDistributed { get; set; }
        public Vector3 StartPosition { get; private set; }

        private void Start()
        {
            _figureView = GetComponentInChildren<FigureView>();
            _figureView.UpdateFigureSprite(_figureData);

            StartPosition = transform.position;
        }

        public void OnPointerClick(PointerEventData eventData) => LockFigure(!IsLocked);

        public void LockFigure(bool isLocked)
        {
            if (BattleState.State != GameState.GameState.Play) return;

            IsLocked = isLocked;
            _figureView.ScaleOutline(isLocked);
        }

        public void SetActiveFigure(bool isActive) => gameObject.SetActive(isActive);
    }
}