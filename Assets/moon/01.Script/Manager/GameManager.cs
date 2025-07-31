using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using moon._01.Script.SO;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;
using UnityEngine.Serialization;

namespace moon._01.Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        public int Wave { get; private set; } = 1;
        [SerializeField] private ScriptFinderSO spawnManagerFinder;
        [SerializeField] private ScriptFinderSO scoreManagerFinder;

        [field: SerializeField ,SerializedDictionary("Wave", "EnemyList")]
        public SerializedDictionary<int, WaveDataListSO> waveEnemy {get; private set;}
        public EnemySpawnManager SpawnManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }

        public Action OnGameEndEvent;

        private void Awake()
        {
            SpawnManager = spawnManagerFinder.GetTarget<EnemySpawnManager>();
            ScoreManager = scoreManagerFinder.GetTarget<ScoreManager>();
            ScoreManager.Initialize(this);
            SpawnManager.NextWaveEvent += NextWave;
            ResetWave();
        }

        private void OnDestroy()
        {
            SpawnManager.NextWaveEvent -= NextWave;
        }

        public void NextWave()
        {
            Wave++;
            var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
            SpawnManager.SetNextWave(spawnCount,spawnTime,enemyPrefabs);
        }

        public void ResetWave()
        {
            Wave = 1;
            var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
            SpawnManager.ResetSpawnManager(spawnCount,spawnTime,enemyPrefabs);
            ScoreManager.ResetScoreManager();
        }

        public (int,float,List<GameObject>) GetWaveData()
        {
            if (waveEnemy.ContainsKey(Wave))
            {
                WaveDataListSO waveDataList = waveEnemy[Wave];
                return (waveDataList.EnemySpawnCount, waveDataList.SpawnTime ,waveDataList.EnemyPrefabs);
            }
            OnGameEndEvent?.Invoke();
            return (0, 0, new List<GameObject>());
        }
    }
}
