using System;
using UnityEngine;

namespace moon._01.Script.Enemys
{
    public class EnemyAnimator : MonoBehaviour,IEntityCompo
    {
        [SerializeField] private Animator animator;
        private string _currentAniName;

        private Enemy _enemy;
        public event Action OnDieAnimationEndEvent;
        
        public event Action OnAttackAnimationEndEvent;
        public event Action OnAttackEvent;
        
        [ContextMenu("Die Ani End")]
        public void AnimationEnd()
        {
            OnDieAnimationEndEvent?.Invoke();
        }

        public void AttackAnimationEnd()
        {
            OnAttackAnimationEndEvent?.Invoke();
        }

        public void Attack()
        {
            OnAttackEvent?.Invoke();
        }

        public void ChangeAnimation(string aniName)
        {
            Debug.Log($"ChangeAnimation 호출: 이전[{_currentAniName}] → 새[{aniName}]");
            if(!string.IsNullOrEmpty(_currentAniName))
               animator.SetBool(_currentAniName, false); 
            animator.SetBool(aniName,true);
            _currentAniName = aniName;
        }

        public void Initialize(Enemy enemy)
        {
            _enemy = enemy;

             if(enemy.target.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z); 
            }
        }

        public void Desolve()
        {
            
        }
    }
}
