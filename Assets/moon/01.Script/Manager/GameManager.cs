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

        [field: SerializeField]
        public List<WaveDataListSO> WaveEnemy {get; private set;}
        
        [field: SerializeField]
        public WaveDataListSO BossEnemy {get; private set;}
        public EnemySpawnManager SpawnManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }

        public bool AllKillBoss { get; private set; } = false;

        public Action OnCutSceneEnd;

        public event Action OnBossScene;

        public event Action OnGameEndEvent;

        private void Awake()
        {
            SpawnManager = spawnManagerFinder.GetTarget<EnemySpawnManager>();
            ScoreManager = scoreManagerFinder.GetTarget<ScoreManager>();
            ScoreManager.Initialize(this);
            SpawnManager.NextWaveEvent += NextWave;
            OnCutSceneEnd += SetNextWave;
            ResetWave();
        }

        private void OnDestroy()
        {
            SpawnManager.NextWaveEvent -= NextWave;
            OnCutSceneEnd -= SetNextWave;
        }

        public void NextWave()
        {
            Wave++;
            if (!AllKillBoss && Wave <= WaveEnemy.Count)
            {
                var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
                SpawnManager.SetNextWave(spawnCount,spawnTime,enemyPrefabs);
            }
            else
            {
                OnBossScene?.Invoke();
            }
        }

        public void SetNextWave()
        {
            var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
            SpawnManager.SetNextWave(spawnCount,spawnTime,enemyPrefabs);
        }

        public void ResetWave()
        {
            Wave = 1;
            var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
            SpawnManager.ResetSpawnManager(spawnCount,spawnTime,enemyPrefabs);
            ScoreManager.ResetScoreManager();
            AllKillBoss = false;
        }

        public (int,float,List<GameObject>) GetWaveData()
        {
            if (Wave <= WaveEnemy.Count)
            {
                WaveDataListSO waveDataList = WaveEnemy[Wave - 1];
                return (waveDataList.EnemySpawnCount, waveDataList.SpawnTime ,waveDataList.EnemyPrefabs);
            }

            if (!AllKillBoss)
            {
                AllKillBoss = true;
                return (BossEnemy.EnemySpawnCount, BossEnemy.SpawnTime ,BossEnemy.EnemyPrefabs);
            }

            OnGameEndEvent?.Invoke();
            return (0, 0, new List<GameObject>());
        }
    }
}
