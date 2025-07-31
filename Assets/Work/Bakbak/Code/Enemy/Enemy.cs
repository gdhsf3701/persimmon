using Plugins.ScriptFinder.RunTime.Finder;
using System;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class Enemy : MonoBehaviour
{
    public event Action OnHit;
    public event Action<Enemy> OnDeadEvent;

    private IEntityCompo[] Compos;

    [SerializeField]
    private int reward = 10;

    [SerializeField]
    private ScriptFinderSO finder;

    public int Reward { get => reward; private set => reward = value; }

    [ContextMenu("spawn")]
    public void Spawned()
    {
        SetCompo();
        OnDeadEvent += finder.GetTarget<ComboManager>().Kill;
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
        OnDeadEvent -= finder.GetTarget<ComboManager>().Kill;

        foreach (IEntityCompo compo in Compos)
        {
            compo.Desolve();
        }

        Destroy(gameObject);
    }
}
