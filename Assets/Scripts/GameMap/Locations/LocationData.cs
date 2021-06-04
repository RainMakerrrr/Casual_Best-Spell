using GameMap.Data;
using Items;
using UnityEngine;

namespace GameMap.Locations
{
    [CreateAssetMenu(fileName = "New Location Data", menuName = "Data / Location Data")]
    public class LocationData : ScriptableObject
    {
        [SerializeField] private QuestItem _questItem;
        [SerializeField] private MapData _mapData;
        [SerializeField] private int _locationNumber;
        [SerializeField] private string _locationScene;
        [SerializeField] private bool _isPassed;
        [SerializeField] private int _locationReward;

        public QuestItem QuestItem => _questItem;

        public int LocationNumber => _locationNumber;
        public string LocationScene => _locationScene;
        public int LocationReward => _locationReward;

        public bool IsPassed => _isPassed;


        public void UpdateMapData()
        {
            var nextLocationNumber = _locationNumber + 1;
            _mapData.SetUnlockedLevel(nextLocationNumber);
            _isPassed = true;
        }
    }
}