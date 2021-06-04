using GameEconomy;
using GameState;
using TMPro;
using UnityEngine;

namespace GameMap.Locations
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private LocationData _locationData;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        private PlayerGold _playerGold;

        private readonly int _locationReward = 100;

        public LocationData LocationData => _locationData;

        private void Start()
        {
            _playerGold = FindObjectOfType<PlayerGold>();
            _rewardText.text = _locationReward.ToString();

            BattleState.OnLocationPassed += GiveReward;
        }

        private void GiveReward() => _playerGold.AddGold(_locationReward);
    }
}