using System;
using GameMap.Data;
using NuclearBand;
using UnityEngine;

namespace SaveSystem
{
    public class TestDataUpdater : MonoBehaviour
    {
        [SerializeField] private TestData _testData;

        private static TestDataUpdater _instance;

        // private async void Awake()
        // {
        //     if (!_instance)
        //         _instance = this;
        //     else Destroy(gameObject);
        //
        //     DontDestroyOnLoad(this);
        //
        //     SODatabase.Init(null, () =>
        //     {
        //         var mapData = SODatabase.GetModel<MapData>(_mapData.FullPath);
        //         Debug.Log(mapData.UnlockedLevels);
        //     });
        //     //await SODatabase.LoadAsync();
        // }

        private void Start()
        {
            SODatabase.Init(null, null);
        }

        [ContextMenu("Get Data")]
        private void TestGetData()
        {
            var mapData = SODatabase.GetModel<TestData>(_testData.Path);
            Debug.Log(mapData.Order);
        }

        private void OnApplicationQuit()
        {
            SODatabase.Save();
        }
    }
}