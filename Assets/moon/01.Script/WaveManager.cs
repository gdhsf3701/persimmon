using System;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace moon._01.Script
{
    public class WaveManager : MonoBehaviour
    {
        public int Wave { get; private set; } = 1;
        [SerializeField] private int spawnMany;
        [SerializeField] private float spawnMultiplyToWave;
        [SerializeField] private ScriptFinderSO spawnManagerFinder;
        private EnemySpawnManager _spawnManager;

        private void Awake()
        {
            _spawnManager = spawnManagerFinder.GetTarget<EnemySpawnManager>();
            ResetWave();
        }
        
        public void NextWave()
        {
            Wave++;
            _spawnManager.SetSpawnCount(GetWaveToSpawnCount());
        }

        public void ResetWave()
        {
            Wave = 1;
            _spawnManager.ResetSpawnManager(GetWaveToSpawnCount());
        }

        public int GetWaveToSpawnCount()
        {
            int value = Mathf.FloorToInt(spawnMany * ((spawnMultiplyToWave - 1) * (Wave - 1) + 1));
            return value;
        }
    }
}
