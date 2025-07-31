using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace moon._01.Script
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float distance;
        [SerializeField] private int many;
        [SerializeField] private float spawnTime;
        private float _timeMultiply;
        private float _timer = 0;
        private int _enemyCount = 0;

        public event Action NextWaveEvent;
        public int SpawnCount { get; private set; } = 0;

        public void ResetSpawnManager(int spawnCount = 1)
        {
            _timer = 0;
            _timeMultiply = 1;
            _enemyCount = 0;
            SpawnCount = spawnCount;
        }

        private void EnemyDie(Enemy enemy)
        {
            _enemyCount--;
            NextWaveEvent?.Invoke();
            enemy.OnDeadEvent -= EnemyDie;
        }

        public void SetSpawnCount(int spawnCount)
        {
            SpawnCount = spawnCount;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (SpawnCount > 0 && _timer >= spawnTime * _timeMultiply)
            {
                _timer = 0;
                int rand = Random.Range(0, many);
                Enemy obj = Instantiate(enemyPrefab , IntToPos(rand) ,Quaternion.identity).GetComponent<Enemy>();
                obj.Spawned();
                _enemyCount++;
                obj.OnDeadEvent += EnemyDie;
                SpawnCount--;
            }
        }

        private Vector3 IntToPos(int value)
        {
            float angle = value * Mathf.PI * 2 / many;
            Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
            return pos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < many; i++)
            {
                float angle = i * Mathf.PI * 2 / many;
                Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
}
