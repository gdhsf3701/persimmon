using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace moon._01.Script.SO
{
    [CreateAssetMenu(fileName = "Wave1Enemy", menuName = "SO/List/Enemy", order = 0)]
    public class WaveDataListSO : ScriptableObject
    {
        [field: SerializeField]public int EnemySpawnCount {get; private set;}
        [field: SerializeField]public float SpawnTime {get; private set;}
        [field: SerializeField]public List<GameObject> EnemyPrefabs {get; private set;}
    }
}