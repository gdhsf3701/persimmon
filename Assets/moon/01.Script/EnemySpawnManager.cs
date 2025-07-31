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
        private float _timer = 0;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= spawnTime)
            {
                _timer = 0;
                int rand = Random.Range(0, many);
                Instantiate(enemyPrefab , GetPosToInt(rand) ,Quaternion.identity);
            }
        }

        private Vector3 GetPosToInt(int value)
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
