using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class Enemy : MonoBehaviour
{
    public event Action OnDespawn;

    public event Action<ShapeSO> OnHit;
    public event Action<ShapeSO> OnDeadEvent;
    public void Spawned()
    {
        SetCompo();
    }

    private void SetCompo()
    {
        var compos = GetComponentsInChildren<IEntityCompo>();
        foreach (IEntityCompo compo in compos)
        {
            compo.Initialize(this);
        }
    }
    public void Despawned()
    {
        OnDespawn?.Invoke();
    }

    public void OnDead()
    {

    }
}
