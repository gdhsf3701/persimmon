using System;
using moon._01.Script.Manager;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace moon._01.Script.CutScene
{
    public class CutScene : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO gameManagerFinder;
        [SerializeField] private CutSceneAnimator cutSceneAnimator;
        private void Awake()
        {
            gameManagerFinder.GetTarget<GameManager>().OnBossScene += BossSceneHandle;
            cutSceneAnimator.OnAnimationEndEvent += HandleAnimationEnd;
            cutSceneAnimator.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            gameManagerFinder.GetTarget<GameManager>().OnBossScene -= BossSceneHandle;
            cutSceneAnimator.OnAnimationEndEvent -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd()
        {
            cutSceneAnimator.gameObject.SetActive(false);
            gameManagerFinder.GetTarget<GameManager>().OnCutSceneEnd?.Invoke();
        }

        private void BossSceneHandle(int obj)
        {
            cutSceneAnimator.gameObject.SetActive(true);
            cutSceneAnimator.PlayAnimator(obj.ToString());
        }
    }
}
