using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class EnemyHealth : MonoBehaviour, IEntityCompo
{
    private Enemy owner;
    private HeartUI heartui;

    [SerializeField]
    private List<ShapType> hearts;
    public void ApplyDamage(ShapType shape)
    {
        if(owner.IsDead)
            return;
        if(shape == hearts[0])
        {
            hearts.RemoveAt(0);
            heartui.SetAllHeart(hearts);
        }
        if(hearts.Count == 0)
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

        heartui = owner.GetComponentInChildren<HeartUI>();
        heartui.SetAllHeart(hearts);
    }
}
