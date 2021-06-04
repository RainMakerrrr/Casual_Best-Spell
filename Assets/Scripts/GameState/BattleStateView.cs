using Player;
using UnityEngine;
using UnityEngine.UI;

namespace GameState
{
    public class BattleStateView : MonoBehaviour
    {
        [SerializeField] private Image _winPanel;
        [SerializeField] private Image _losePanel;

        private void OnEnable()
        {
            BattleState.OnLocationPassed += EnableWinPanel;
            PlayerStats.OnDie += EnableLosePanel;
        }

        private void EnableWinPanel() => _winPanel.gameObject.SetActive(true);

        private void EnableLosePanel() => _losePanel.gameObject.SetActive(true);

        private void OnDisable()
        {
            BattleState.OnLocationPassed -= EnableWinPanel;
            PlayerStats.OnDie -= EnableLosePanel;
        }
    }
}