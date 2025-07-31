using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class EnemyHealth : MonoBehaviour, IEntityCompo
{
    private Enemy owner;

    [SerializeField]
    private List<ShapType> Hearts;

    public void ApplyDamage(ShapType shape)
    {
        if(shape == Hearts[0])
        {
            Hearts.RemoveAt(0);
        }

        if(Hearts.Count == 0)
        {
            owner.OnDead();
        }
    }

    public void Desolve()
    {
        AttackManager.Instance.OnAttack -= ApplyDamage;
    }

    public void Initialize(Enemy enemy)
    {
        owner = enemy;

        AttackManager.Instance.OnAttack += ApplyDamage;
    }
}
