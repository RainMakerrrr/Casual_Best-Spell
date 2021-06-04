using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Figures;
using GameState;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Figures
{
    public class FiguresSpawner : MonoBehaviour
    {
        public static event Action OnTurnPassed;

        [SerializeField] private FigureData[] _figuresData;
        [SerializeField] private List<Figure> _figures = new List<Figure>();

        public List<Figure> UnholdedFigures => _figures.Where(figure => !figure.IsLocked).ToList();

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            _figures = FindObjectsOfType<Figure>().ToList();
            GetComponent<FiguresDistributor>();

            BattleState.OnRoundWin += UpdateFigures;


            _figures.ForEach(figure => figure.FigureView.gameObject.SetActive(false));
        }

        public void UnholdFigures() => _figures.ForEach(figure => figure.LockFigure(false));

        public void SpawnFigure()
        {
            if (BattleState.State == GameState.GameState.Rotating) return;

            var unholdedFigures = _figures.Where(figure => !figure.IsLocked).ToList();

            SetupFigures(unholdedFigures);
            OnTurnPassed?.Invoke();
        }

        public void SpawnDisableFigures()
        {
            if (BattleState.State == GameState.GameState.Rotating) return;
            var disabledFigures = _figures.Where(figure => !figure.gameObject.activeSelf).ToList();

            Debug.Log(disabledFigures.Count);

            if (disabledFigures.Count <= 0) return;
            disabledFigures.ForEach(figure => figure.gameObject.SetActive(true));

            SetupFigures(disabledFigures);
        }

        private void SetupFigures(IEnumerable<Figure> figures)
        {
            foreach (var figure in figures)
            {
                figure.SetActiveFigure(true);
                figure.FigureView.gameObject.SetActive(true);

                var starFigure = _figuresData.FirstOrDefault(f => f.FigureType == FigureType.Star);

                if (Random.Range(0, 101) > 98)
                    figure.FigureData = starFigure;
                else
                {
                    var newFigures = _figuresData.Where(f => f != starFigure).ToList();
                    figure.FigureData = newFigures[Random.Range(0, newFigures.Count)];
                }

                figure.FigureView.UpdateFigureSprite(figure.FigureData);
            }
        }

        private void UpdateFigures()
        {
            var unholdedFigures = _figures.Where(figure => !figure.IsLocked).ToList();
            SetupFigures(unholdedFigures);
        }
    }
}