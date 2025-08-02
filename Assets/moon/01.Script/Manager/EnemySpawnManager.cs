using System;
using System.Collections.Generic;
using CSI._07_Shader.Fade;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace moon._01.Script.Manager
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private int many;
        [SerializeField] private float spawnTime;
        [SerializeField] private FadeScreenManager fadeScreenManager;
        [SerializeField] private bool isUpSpawn = false;
        [SerializeField] private bool isSildeSpawn = false;
        [SerializeField] private bool isTutorial;
        private bool isSpawnedBoss = false;
        [SerializeField]private bool is2Stage = false;
        private List<GameObject> _enemyPrefabs = new List<GameObject>();
        
        private float _timer = 0;
        private float _Alltimer = 0;
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
            _Alltimer = 0;
        }
        
        public void SetNextWave(int spawnCount , float time , List<GameObject> enemyPrefabs,bool isBoolWave = false)
        {
            Debug.Log("SetNextWave");
            SpawnCount = spawnCount;
            _enemyPrefabs = enemyPrefabs;
            spawnTime = time;
            _timer = 0;
            if (isBoolWave)
            {
                _Alltimer -= 100;
            }
            else
            {
                _Alltimer -= 3;
            }

            if (isBoolWave)
            {
                isSpawnedBoss = true;
            }
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
            if (_Alltimer - 2 >= 0)
            {
                _Alltimer -= 2;

            }
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

            if (is2Stage)
            {
                _Alltimer += Time.deltaTime/0.8f;
            }
            else
            {
                _Alltimer += Time.deltaTime/1f;
            }
            _timer += Time.deltaTime;
            if (SpawnCount > 0 && _timer >= spawnTime)
            {
                _timer = 0;
                SpawnEnemy();
            }
            if(!isTutorial)
                fadeScreenManager.SetFade(2.2f-(_Alltimer / 15 * 2.2f));
        }

        private void SpawnEnemy()
        {
            int rand = Random.Range(0, many);
            int enemyRand = Random.Range(0, _enemyPrefabs.Count);
            Enemy obj;
            if (!isUpSpawn)
            {
                obj = Instantiate(_enemyPrefabs[enemyRand], IntToPos(rand), Quaternion.identity).GetComponent<Enemy>();
            }else if (isSildeSpawn && isSpawnedBoss)
            {
                obj = Instantiate(_enemyPrefabs[enemyRand], Vector2.right * distance, Quaternion.identity).GetComponent<Enemy>();
                Debug.Log("isSildeSpawn");
            }
            else if(isUpSpawn)
            {
                obj = Instantiate(_enemyPrefabs[enemyRand], Vector2.up * distance, Quaternion.identity).GetComponent<Enemy>();
            }
            else
            {
                obj = Instantiate(_enemyPrefabs[enemyRand], IntToPos(rand), Quaternion.identity).GetComponent<Enemy>();
            }
            obj.Spawned();
            _enemyCount++;
            _actionEnemy.Add(obj);
            obj.OnDeadEvent += EnemyDie;
            SpawnCount--;
        }

        public Enemy SpawnDeputyEnemy(GameObject prefab, int i = -1)
        {
            int pos;
            if (i == -1)
                pos = Random.Range(0, many);
            else
                pos = i;
            Enemy obj = Instantiate(prefab , IntToPos(pos) ,Quaternion.identity).GetComponent<Enemy>();
            obj.Spawned();
            return obj;
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
            if (!isUpSpawn)
            {
                for (int i = 0; i < many; i++)
                {
                    float angle = i * Mathf.PI * 2 / many;
                    Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
                    Gizmos.DrawWireSphere(pos, 0.5f);
                }
            }
            else
            {
                Vector3 pos = Vector2.up * distance;
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
}
