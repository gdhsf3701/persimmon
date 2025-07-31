using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace moon._01.Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        public int Wave { get; private set; } = 1;
        [SerializeField] private int spawnMany;
        [SerializeField] private float spawnMultiplyToWave;
        [SerializeField] private ScriptFinderSO spawnManagerFinder;
        [SerializeField] private ScriptFinderSO scoreManagerFinder;
        public EnemySpawnManager SpawnManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }

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
            SpawnManager.SetSpawnCount(GetWaveToSpawnCount());
        }

        public void ResetWave()
        {
            Wave = 1;
            SpawnManager.ResetSpawnManager(GetWaveToSpawnCount());
            ScoreManager.ResetScoreManager();
        }

        public int GetWaveToSpawnCount()
        {
            int value = Mathf.FloorToInt(spawnMany * ((spawnMultiplyToWave - 1) * (Wave - 1) + 1));
            return value;
        }
    }
}
