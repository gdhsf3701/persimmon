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
        public int SpawnCount { get; private set; } = 0;

        public void ResetSpawnManager(int spawnCount = 1)
        {
            _timer = 0;
            _timeMultiply = 1;
            SpawnCount = spawnCount;
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
                GameObject obj = Instantiate(enemyPrefab , IntToPos(rand) ,Quaternion.identity);
                obj.GetComponent<Enemy>().Spawned();
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
