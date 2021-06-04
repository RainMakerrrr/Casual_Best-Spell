using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enemies;
using Figures;
using GameMap.Locations;
using GameMap.Quests;
using Player;
using UnityEngine;

namespace GameState
{
    public class BattleState : MonoBehaviour
    {
        public static event Action OnLocationPassed;
        public static event Action OnRoundWin;
        public static event Action<int> OnTurnIncremented;
        public static GameState State { get; set; } = GameState.Start;
        public static BattleState Instance { get; private set; }

        [SerializeField] private List<Enemy> _nextEnemies = new List<Enemy>();
        private List<Enemy> _enemies = new List<Enemy>();


        private int _turn;
        private int _enemiesCount;

        private Camera _camera;
        private Location _location;

        private void OnEnable()
        {
            Instance = this;

            State = GameState.Start;

            _camera = Camera.main;
            _enemies = FindObjectsOfType<Enemy>().ToList();
            _location = FindObjectOfType<Location>();

            _turn = -2;
            FiguresSpawner.OnTurnPassed += IncrementTurn;
            PlayerStats.OnDie += SetLoseState;
        }

        private void IncrementTurn()
        {
            _turn++;
            OnTurnIncremented?.Invoke(_turn);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);

            _enemiesCount++;
            PlayerQuests.Instance.CheckDefeatEnemiesQuests(_location.LocationData, _enemiesCount);

            if (_enemies.Count <= 0)
                LoadNextEnemy();
        }

        private void LoadNextEnemy()
        {
            State = GameState.Rotating;
            _turn = 1;

            var nextEnemy = _nextEnemies.FirstOrDefault();
            if (nextEnemy == null)
            {
                _location.LocationData.UpdateMapData();
                State = GameState.Win;

                if (_location.LocationData.QuestItem != null)
                    PlayerQuests.Instance.CheckFindItemQuests(_location.LocationData, _location.LocationData.QuestItem);

                OnLocationPassed?.Invoke();
                return;
            }

            _camera.transform.DOMoveX(_camera.transform.position.x + 5.6f, 2f)
                .OnComplete(() =>
                {
                    State = GameState.Play;
                    OnRoundWin?.Invoke();
                });

            nextEnemy.gameObject.SetActive(true);

            _enemies.Add(nextEnemy);
            _nextEnemies.Remove(nextEnemy);
        }

        private void SetLoseState() => State = GameState.Lose;

        private void OnDisable()
        {
            FiguresSpawner.OnTurnPassed -= IncrementTurn;
            PlayerStats.OnDie -= SetLoseState;
        }
    }

    public enum GameState
    {
        Start,
        Map,
        Play,
        Rotating,
        Win,
        Lose
    }
}