using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace moon._01.Script.CutScene
{
    public class CutSceneAnimator : MonoBehaviour
    {
        public event Action OnAnimationEndEvent;
        [SerializeField]private Animator animator;
        private string _currentAnimationName;

        public void PlayAnimator(string name)
        {
            if(!string.IsNullOrEmpty(_currentAnimationName))
                animator.SetBool(_currentAnimationName, false);
            animator.SetBool(name, true);
            _currentAnimationName = name;
        }
        
        
        [ContextMenu("ani end")]
        public void EndAnimation()
        {
            if(!string.IsNullOrEmpty(_currentAnimationName))
                animator.SetBool(_currentAnimationName, false);
            _currentAnimationName = null;
            OnAnimationEndEvent?.Invoke();
        }
    }
}