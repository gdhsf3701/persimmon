using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using moon._01.Script.Manager;
using Plugins.ScriptFinder.RunTime.Finder;
using TMPro;
using UnityEngine;

namespace moon._01.Script.Tutorial
{
    public enum TutorialType
    {
        Enemy,
        Say
    }

    [System.Serializable]
    public struct TutorialObj
    {
        public TutorialType TutorialType;

        //Say
        [TextArea] public string SayText;
        public bool EnemyDieToText;
        public float TextTime;
        public float TextDelayTime;
        public bool EndToDestroy;

        //Enemy
        public GameObject EnemyPrefab;

        [Tooltip("is this value -1 Random Spawn")]
        public int SpawnPointInt;

        public float EnemyStopTime;
    }

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private List<TutorialObj> Tutorial;
        [SerializeField] private ScriptFinderSO gameManagerFinder;
        [SerializeField] private TextMeshProUGUI text;

        public Action TutorialCompleted;

        private int _index = 0;
        private bool _enemyDied = true;

        private void Start()
        {
            StartTutorial();
        }

        private async void StartTutorial()
        {
            while (_index < Tutorial.Count)
            {
                var obj = Tutorial[_index];

                switch (obj.TutorialType)
                {
                    case TutorialType.Say:
                        await HandleText(obj);
                        break;
                    case TutorialType.Enemy:
                        await HandleEnemy(obj);
                        break;
                }

                _index++;
            }

            TutorialCompleted?.Invoke();
        }

        private async Task HandleText(TutorialObj obj)
        {
            if (!obj.EnemyDieToText)
            {
                text.text = "";
                await text.DOText(obj.SayText, obj.TextTime).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }
            else
            {
                while (!_enemyDied)
                    await Task.Delay(200);
                text.text = "";
                await text.DOText(obj.SayText, obj.TextTime).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }

            await Task.Delay((int)(obj.TextDelayTime * 1000));

            if (obj.EndToDestroy)
            {
                text.text = "";
            }
        }

        private async Task HandleEnemy(TutorialObj obj)
        {
            _enemyDied = false;

            var enemy = gameManagerFinder.GetTarget<GameManager>()
                .SpawnManager.SpawnDeputyEnemy(obj.EnemyPrefab, obj.SpawnPointInt);

            enemy.OnDeadEvent += OnEnemyDie;

            await Task.Delay((int)(obj.EnemyStopTime * 1000));
            enemy.GetComponentInChildren<EnemyMover>().SetTargetPos(enemy.transform.position);
        }

        private void OnEnemyDie(Enemy enemy)
        {
            _enemyDied = true;
            enemy.OnDeadEvent -= OnEnemyDie;
        }
    }
}
