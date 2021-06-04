using TMPro;
using UnityEngine;

namespace GameEconomy
{
    public class PlayerGoldView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _goldText;
        private PlayerGold _playerGold;

        private void OnEnable()
        {
            _playerGold = GetComponent<PlayerGold>();
            _playerGold.OnGoldChanged += GoldChangedHandler;
        }

        private void GoldChangedHandler(int gold)
        {
            _goldText.text = gold.ToString();
        }

        private void OnDisable()
        {
            _playerGold.OnGoldChanged -= GoldChangedHandler;
        }
    }
}