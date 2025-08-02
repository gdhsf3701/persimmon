using System;
using UnityEngine;

public class EnemyMover : MonoBehaviour, IEntityCompo
{
    private Enemy owner;
    private Vector2 moveDir = Vector2.zero;
    [SerializeField]
    private float moveSpeed;

    [SerializeField] private float distanceToAttack = 0.25f;

    [SerializeField] private float dieUpSpeed = 1.25f;
    private bool acivate = false;

    public event Action OnAttackEvent;
    
    private Vector2 _target;
    void Update()
    {
        if(acivate == false)
            return;
        if (owner.IsDead)
        {
            transform.position += Vector3.up * dieUpSpeed * Time.deltaTime;
            return;
        }

        if (!CheckTargetInAttackRange())
        {
            transform.position += (Vector3) moveDir * moveSpeed * Time.deltaTime;
        }
        else if(!owner.isAttacking && owner.AttackTimeDone())
        {
            owner.isAttacking = true;
            OnAttackEvent?.Invoke();
        }
    }

    public void SetTargetPos()
    {
        moveDir = (_target -(Vector2)transform.position).normalized;
    }
    
    public void SetTargetPos(Vector2 pos)
    {
        moveDir = (pos -(Vector2)transform.position).normalized;
    }

    public bool CheckTargetInAttackRange()
    {
        return Vector2.Distance(transform.position, _target) <= distanceToAttack;
    }

    public void Initialize(Enemy enemy)
    {
        acivate = true;
        owner = enemy;
        _target = owner.PlayerFinder.GetTargetTransform().position;
        SetTargetPos();
    }

    public void Desolve()
    {
        acivate = false ;
    }
}
