using System;
using GameState;
using UnityEngine;
using UnityEngine.UI;

namespace Figures
{
    public class RollButton : MonoBehaviour
    {
        [SerializeField] private ButtonState _buttonState = ButtonState.Start;

        private FiguresDistributor _figuresDistributor;
        private FiguresSpawner _figuresSpawner;
        private Button _rollButton;

        private void OnEnable()
        {
            _figuresDistributor = FindObjectOfType<FiguresDistributor>();
            _figuresSpawner = FindObjectOfType<FiguresSpawner>();

            _rollButton = GetComponent<Button>();
            _rollButton.onClick.AddListener(RollButtonClickHandler);
            BattleState.OnRoundWin += ChangeState;
        }

        private void RollButtonClickHandler()
        {
            switch (_buttonState)
            {
                case ButtonState.Start:
                    _buttonState = ButtonState.Roll;
                    _figuresSpawner.SpawnFigure();
                    BattleState.State = GameState.GameState.Play;
                    break;
                case ButtonState.Deal:
                    _buttonState = ButtonState.Roll;
                    _figuresSpawner.SpawnFigure();
                    _figuresSpawner.UnholdFigures();
                    break;
                case ButtonState.Roll:

                    if (!_figuresDistributor.HasCombinations()) goto case ButtonState.Deal;

                    _figuresDistributor.DistributeFigures();
                    _figuresSpawner.UnholdFigures();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeState() => _buttonState = ButtonState.Roll;

        private void OnDisable()
        {
            _rollButton.onClick.RemoveListener(RollButtonClickHandler);
            BattleState.OnRoundWin -= ChangeState;
        }
    }

    public enum ButtonState
    {
        Start,
        Deal,
        Roll
    }
}