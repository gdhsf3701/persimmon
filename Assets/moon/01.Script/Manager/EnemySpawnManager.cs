using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace moon._01.Script.Manager
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private int many;
        [SerializeField] private float spawnTime;
        private List<GameObject> _enemyPrefabs = new List<GameObject>();
        private float _timer = 0;
        private int _enemyCount = 0;

        public event Action<int> OnScoreChangeEvent;
        public event Action NextWaveEvent;
        public int SpawnCount { get; private set; } = 0;

        private List<Enemy> _actionEnemy = new List<Enemy>();

        public void ResetSpawnManager(int spawnCount , float time , List<GameObject> enemyPrefabs)
        {
            _timer = 0;
            _enemyCount = 0;
            SpawnCount = spawnCount;
            spawnTime = time;
            _enemyPrefabs = enemyPrefabs;
        }
        
        public void SetNextWave(int spawnCount , float time , List<GameObject> enemyPrefabs)
        {
            SpawnCount = spawnCount;
            _enemyPrefabs = enemyPrefabs;
            spawnTime = time;
            _timer = 0;
        }

        private void EnemyDie(Enemy enemy)
        {
            _enemyCount--;
            OnScoreChangeEvent?.Invoke(10);
            if (_enemyCount <= 0)
            {
                _enemyCount = 0;
                NextWaveEvent?.Invoke();
            }
            _actionEnemy.Remove(enemy);
            enemy.OnDeadEvent -= EnemyDie;
        }

        private void OnDestroy()
        {
            foreach (var enemy in _actionEnemy)
            {
                enemy.OnDeadEvent -= EnemyDie;
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (SpawnCount > 0 && _timer >= spawnTime)
            {
                _timer = 0;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            int rand = Random.Range(0, many);
            int enemyRand = Random.Range(0, _enemyPrefabs.Count);
            Enemy obj = Instantiate(_enemyPrefabs[enemyRand] , IntToPos(rand) ,Quaternion.identity).GetComponent<Enemy>();
            obj.Spawned();
            _enemyCount++;
            _actionEnemy.Add(obj);
            obj.OnDeadEvent += EnemyDie;
            SpawnCount--;
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
