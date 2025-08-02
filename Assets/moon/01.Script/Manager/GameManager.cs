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
        public int CutScene { get; private set; } = 1;
        public int BossWave { get; private set; } = 1;
        public bool AllKillBoss { get; private set; } = false;

        [SerializeField] private ScriptFinderSO spawnManagerFinder;
        [SerializeField] private ScriptFinderSO scoreManagerFinder;
        [field: SerializeField] public List<WaveDataListSO> WaveEnemy { get; private set; }
        [field: SerializeField] public List<WaveDataListSO> BossEnemy { get; private set; }
        [field: SerializeField] public int CutSceneMany { get; private set; }

        public EnemySpawnManager SpawnManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }

        [field: SerializeField] private bool isTutorial;

        public event Action<int> OnBossScene;
        public Action OnCutSceneEnd;
        public event Action OnGameEndEvent;
        public string nextScene;

        private void Awake()
        {
            SpawnManager = spawnManagerFinder.GetTarget<EnemySpawnManager>();
            ScoreManager = scoreManagerFinder.GetTarget<ScoreManager>();
            ScoreManager.Initialize(this);
            SpawnManager.NextWaveEvent += NextWave;
            OnCutSceneEnd += SetNextWave;
            if (!isTutorial)
                ResetWave();
        }

        private void OnDestroy()
        {
            SpawnManager.NextWaveEvent -= NextWave;
        }

        public void NextWave()
        {
            Wave++;
            if (Wave <= WaveEnemy.Count)
            {
                var (count, time, prefabs) = GetWaveData();
                SpawnManager.SetNextWave(count, time, prefabs);
                return;
            }

            if (CutScene < CutSceneMany)
            {
                OnBossScene?.Invoke(CutScene - 1);
                CutScene++;
                return;
            }

            SetNextWave();
        }

        public void SetNextWave()
        {
            if (BossWave > BossEnemy.Count)
            {
                OnBossScene?.Invoke(CutScene - 1);
                CutScene++;
                OnCutSceneEnd -= SetNextWave;
                _ = GetWaveData();
                return;
            }

            var (count, time, prefabs) = GetWaveData();
            BossWave++;
            SpawnManager.SetNextWave(count, time, prefabs,true);
        }

        public void ResetWave()
        {
            Wave = 1;
            BossWave = 1;
            CutScene = 1;
            AllKillBoss = false;
            ScoreManager.ResetScoreManager();
            if (!(Wave <= WaveEnemy.Count) && BossWave <= BossEnemy.Count)
            {
                NextWave();
                return;
            }
            var (count, time, prefabs) = GetWaveData();
            SpawnManager.ResetSpawnManager(count, time, prefabs);
        }

        public (int, float, List<GameObject>) GetWaveData()
        {
            if (Wave <= WaveEnemy.Count)
            {
                WaveDataListSO w = WaveEnemy[Wave - 1];
                return (w.EnemySpawnCount, w.SpawnTime, w.EnemyPrefabs);
            }

            if (BossWave <= BossEnemy.Count)
            {
                WaveDataListSO b = BossEnemy[BossWave - 1];
                return (b.EnemySpawnCount, b.SpawnTime, b.EnemyPrefabs);
            }

            OnGameEndEvent?.Invoke();
            print("end");
            Padeinout._instance.PadeIn(nextScene);
            return (0, 0f, new List<GameObject>());
        }
    }
}
