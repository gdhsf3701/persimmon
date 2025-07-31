using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class Enemy : MonoBehaviour
{
    public event Action OnHit;
    public event Action<Enemy> OnDeadEvent;

    private IEntityCompo[] Compos;

    [ContextMenu("spawn")]
    public void Spawned()
    {
        SetCompo();
    }

    private void SetCompo()
    {
        Compos = GetComponentsInChildren<IEntityCompo>();
        foreach (IEntityCompo compo in Compos)
        {
            compo.Initialize(this);
        }
    }

    public void OnDead()
    {
        OnDeadEvent?.Invoke(this);
        foreach (IEntityCompo compo in Compos)
        {
            compo.Desolve();
        }
    }
}
