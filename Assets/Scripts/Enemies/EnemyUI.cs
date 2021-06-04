using DG.Tweening;
using GameState;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        private Enemy _enemy;
        private Camera _camera;

        private void OnEnable()
        {
            _enemy = GetComponent<Enemy>();
            _camera = Camera.main;

            EnableHealthBar();
            
            BattleState.OnRoundWin += EnableHealthBar;
            _enemy.OnTakeDamage += UpdateHealthBar;
            _enemy.OnDie += DisableHealthBar;
        }

        private void EnableHealthBar()
        {
            _healthBar.gameObject.SetActive(true);
            _healthBar.transform.position =
                _camera.WorldToScreenPoint(transform.position + new Vector3(0f, -1.5f, 0f));
            
            _healthBar.maxValue = _enemy.EnemyData.Health;
            _healthBar.value = _healthBar.maxValue;
        }
        
        private void UpdateHealthBar(int health)
        {
            _healthBar.DOValue(health,2f);
        }

        private void DisableHealthBar(Enemy enemy) => _healthBar.gameObject.SetActive(false);

        private void OnDisable()
        {
            _enemy.OnTakeDamage -= UpdateHealthBar;
            _enemy.OnDie -= DisableHealthBar;
            BattleState.OnRoundWin -= EnableHealthBar;
        }
    }
}