using System.Collections.Generic;
using System.Linq;
using GameMap.Data;
using GameMap.Locations;
using UnityEngine;

namespace GameMap
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private List<LocationPoint> _locationPoints = new List<LocationPoint>();
        [SerializeField] private MapData _mapData;


        private void Start()
        {
            InitializeLocationsPoints(_mapData.UnlockedLevels);
        }

        private void InitializeLocationsPoints(int unlockedLocationsCount)
        {
            _locationPoints = GetComponentsInChildren<LocationPoint>().ToList();

            _locationPoints.Take(unlockedLocationsCount).ToList().ForEach(u => u.IsLocked = false);
            _locationPoints.Skip(unlockedLocationsCount).ToList().ForEach(l => l.IsLocked = true);
        }
    }
}