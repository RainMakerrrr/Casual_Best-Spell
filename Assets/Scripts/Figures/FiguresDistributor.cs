using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Figures;
using DG.Tweening;
using GameState;
using Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Figures
{
    public class FiguresDistributor : MonoBehaviour
    {
        [SerializeField] private Button _rollButton;

        private SpellsDeck _spellsDeck;
        private FiguresSpawner _figuresSpawner;

        private bool _isDistributed = true;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            _spellsDeck = FindObjectOfType<SpellsDeck>();
            _figuresSpawner = GetComponent<FiguresSpawner>();

            BattleState.OnRoundWin += RoundWinHandler;
        }

        public void DistributeFigures() => StartCoroutine(DistributeFiguresRoutine());

        private void RoundWinHandler() => StopCoroutine(DistributeFiguresRoutine());

        private IEnumerator DistributeFiguresRoutine()
        {
            if (BattleState.State == GameState.GameState.Rotating) yield break;

            if (_figuresSpawner.UnholdedFigures.Count <= 0) yield break;

            _isDistributed = false;
            _rollButton.interactable = _isDistributed;

            var sortedSpells = Utilities.QuickSpellSortDescending(_spellsDeck.Spells).ToList();
            Debug.Log(sortedSpells[0].SpellData);

            foreach (var spell in sortedSpells)
            {
                var recipe = GetSpellRecipe(spell);
                if (recipe == null) continue;

                if (recipe.Count != spell.SpellCost)
                    recipe.ForEach(figure => figure.IsDistributed = false);
                else
                {
                    recipe.ForEach(recipePart => CloneAndMoveToSpell(recipePart, spell));
                    yield return new WaitForSeconds(2f);
                }
            }

            _figuresSpawner.UnholdedFigures.ForEach(figure => figure.IsDistributed = false);
            _isDistributed = true;
            _rollButton.interactable = _isDistributed;
        }

        public bool HasCombinations()
        {
            var sortedSpells = Utilities.QuickSpellSortDescending(_spellsDeck.Spells).ToList();

            foreach (var spell in sortedSpells)
            {
                var recipe = GetSpellRecipe(spell);
                if (recipe == null) continue;

                if (recipe.Count != spell.SpellCost)
                    recipe.ForEach(figure => figure.IsDistributed = false);
                else
                {
                    recipe.ForEach(recipePart => CloneAndMoveToSpell(recipePart, spell));
                    return true;
                }
            }

            return false;
        }

        private void CloneAndMoveToSpell(Figure figure, Spell spell)
        {
            if (figure.IsLocked) figure.LockFigure(false);

            var targetImage = spell.SpellView.GetTargetImage(figure.FigureData);
            if (targetImage == null) return;

            figure.FigureView.SetActiveOutline(true);
            targetImage.GetComponent<Outline>().enabled = true;

            var sequence = DOTween.Sequence();
            sequence.Append(
                figure.transform.DOMoveY(figure.transform.position.y + 30f, 0.3f).SetEase(Ease.Flash));
            sequence.Append(figure.transform.DOShakePosition(0.3f, Vector3.up * 20f));


            sequence.OnComplete(() =>
            {
                targetImage.gameObject.SetActive(false);
                figure.FigureView.SetActiveOutline(false);
                figure.FigureView.FigureSprite.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    figure.transform.DOMoveY(figure.transform.position.y - 30f, 0.3f).SetEase(Ease.Flash);
                    DistributeEndHandler(spell, figure);
                });
            });
        }

        private List<Figure> GetSpellRecipe(Spell spell)
        {
            var recipe = new List<Figure>();
            var tempFigures = new List<Figure>();

            tempFigures.AddRange(_figuresSpawner.UnholdedFigures);

            foreach (var recipePart in spell.SpellData.SpellRecipe)
            {
                var figure = tempFigures.Find(f =>
                    (f.FigureData == recipePart || f.FigureData.FigureType == FigureType.Star) && !f.IsDistributed);
                if (figure == null) continue;

                tempFigures.Remove(figure);

                recipe.Add(figure);
                figure.IsDistributed = true;
            }

            return recipe;
        }


        private void DistributeEndHandler(Spell spell, Figure figure)
        {
            spell.AddRecipeElement(figure.FigureData);
            figure.SetActiveFigure(false);
            figure.FigureView.FigureSprite.SetAlpha(1f);
            _figuresSpawner.SpawnDisableFigures();
        }
    }
}