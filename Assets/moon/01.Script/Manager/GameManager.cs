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
        public int CutSceneMany {get; private set;}
        [field: SerializeField]
        public List<WaveDataListSO> BossEnemy {get; private set;}
        public EnemySpawnManager SpawnManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }
        
        
        public int CutScene { get; private set; } = 1;

        public bool AllKillBoss { get; private set; } = false;

        public Action OnCutSceneEnd;

        public event Action<int> OnBossScene;

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
            if (Wave <= WaveEnemy.Count)
            {
                var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
                SpawnManager.SetNextWave(spawnCount,spawnTime,enemyPrefabs);
            }
            else
            {
                OnBossScene?.Invoke(CutScene - 1);
            }
        }

        public void SetNextWave()
        {
            var (spawnCount,spawnTime, enemyPrefabs) = GetWaveData();
            CutScene++;
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

            if (CutSceneMany >= CutScene && BossEnemy.Count >= CutScene)
            {
                return (BossEnemy[CutScene - 1].EnemySpawnCount, BossEnemy[CutScene - 1].SpawnTime,
                    BossEnemy[CutScene - 1].EnemyPrefabs);
            }

            OnGameEndEvent?.Invoke();
            return (0, 0, new List<GameObject>());
        }
    }
}
