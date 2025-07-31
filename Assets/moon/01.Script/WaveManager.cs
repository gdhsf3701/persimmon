using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace moon._01.Script
{
    public class WaveManager : MonoBehaviour
    {
        public int Wave { get; private set; } = 0;
        [SerializeField] private ScriptFinderSO spawnManagerFinder;

        public void NextWave()
        {
            Wave++;
        }

        public void ResetWave()
        {
            spawnManagerFinder.GetTarget<EnemySpawnManager>().ResetSpawnManager();
            Wave = 0;
        }
    }
}
